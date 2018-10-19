﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DsaSoftPanel.Enumeration;
using DsaSoftPanel.FunctionUtility;

namespace DsaSoftPanel.TaskComponents
{
    class FunctionTask
    {
        private Thread _funcThread;
        private readonly SoftPanelGlobalInfo _globalInfo;
        private readonly DsaSoftPanelForm _parentForm;
        private List<double> _dataBuf;

        public MeasureType[] MeasureTypes => _measure.Measures.Keys.ToArray();

        private int _functionType;

        public FunctionType FunctionType
        {
            get { return (FunctionType) _functionType; }
            set
            {
                Thread.VolatileWrite(ref _functionType, (int) value);
                // 如果当前函数类型不匹配则重新创建新的function
                if ((null == _function && FunctionType != FunctionType.None) ||
                    (null != _function && !_function.IsTypeFit(FunctionType)))
                {
                    _function = FunctionBase.CreateFunction(FunctionType, _dataBuf);
                    _parentForm.InitFunctionResultArea(_function);
                    _parentForm.InitFunctionConfigArea(_function);
                    RefreshFunctionConfigArea();
                }
            }
        }

        private FunctionBase _function;
        private readonly MeasureHandler _measure;

        public FunctionTask(DsaSoftPanelForm parentForm)
        {
            _globalInfo = SoftPanelGlobalInfo.GetInstance();
            this._dataBuf = new List<double>(Constants.DefaultDisplayBufSize);
            this._parentForm = parentForm;
            this.FunctionType = FunctionType.None;
            this._measure = new MeasureHandler();
        }

        public void Start()
        {
            _funcThread?.Abort();
//            _function = FunctionBase.CreateFunction(FunctionType);
            _function?.RefreshConfigForm();
            _funcThread = new Thread(TaskWork);
            _funcThread.IsBackground = true;
            _funcThread.Start();
            _globalInfo.EnableChannelCount = _globalInfo.Channels.Count(item => item.Enabled);
            _channelRangeHigh = new double[8];
            _channelRangeLow = new double[8];
        }

        public void AddMeasure(MeasureType type)
        {
            _measure.AddMeasure(type);
        }

        public void RemoveMeasre(MeasureType type)
        {
            _measure.RemoveMeasure(type);
        }

        public void Stop()
        {
            try
            {
                _funcThread?.Abort();
                _funcThread = null;
            }
            catch (Exception ex)
            {
                //ignore
            }
        }

        public void RefreshFunctionConfigArea()
        {
            _function?.RefreshConfigForm();
        }

        private void TaskWork()
        {
            Thread.Sleep(2*(_globalInfo.SamplesPerView/_globalInfo.SampleRate));
            while (true)
            {
                // 清空数据buffer并拷贝最新的数据
                this._dataBuf.Clear();
                bool getLock = false;
                try
                {
                    _globalInfo.DispBufLock.Enter(ref getLock);
                    this._dataBuf.AddRange(_globalInfo.DispBuf);
                }
                finally
                {
                    if (getLock)
                    {
                        _globalInfo.DispBufLock.Exit();
                    }
                }

                int samplesInChart = _globalInfo.SamplesInChart;
                if (samplesInChart <= 0)
                {
                    Thread.Sleep(Constants.FuncWaitTime);
                    continue;
                }
                if (_globalInfo.EnableRange)
                {
                    ShowDataRange();
                }

                int measureIndex = _parentForm.GetMeasureIndex();
                if (measureIndex >= 0)
                {
                    string[] result = _measure?.Execute(_dataBuf.GetRange(samplesInChart * measureIndex,
                    samplesInChart).ToArray());
                    _parentForm.RefreshMeasureResult(result);
                }
                

                _function?.ExecuteAndShow();
                Thread.Sleep(Constants.FuncWaitTime);
            }
        }

        private double[] _channelRangeHigh;
        private double[] _channelRangeLow;

        private void ShowDataRange()
        {
            if (_dataBuf.Count == 0)
            {
                return;
            }
            for (int i = 0; i < _channelRangeHigh.Length; i++)
            {
                _channelRangeHigh[i] = double.MinValue;
                _channelRangeLow[i] = double.MaxValue;
            }
            int samplesPerView = _dataBuf.Count/_globalInfo.EnableChannelCount;
            Parallel.For(0, samplesPerView, CalcMaxAndMin);
            _parentForm.Invoke(_globalInfo.ShowRange, _channelRangeHigh, _channelRangeLow);
        }

        private void CalcMaxAndMin(int sampleIndex)
        {
            int samplesPerView = _dataBuf.Count / _globalInfo.EnableChannelCount;
            int pointIndex = sampleIndex;
            for (int i = 0; i < _globalInfo.EnableChannelCount; i++)
            {
                if (_dataBuf[pointIndex] > _channelRangeHigh[i])
                {
                    _channelRangeHigh[i] = _dataBuf[pointIndex];
                }
                else if (_dataBuf[pointIndex] < _channelRangeLow[i])
                {
                    _channelRangeLow[i] = _dataBuf[pointIndex];
                }
                pointIndex += samplesPerView;
            }
        }
    }
}
