﻿using System.Collections.Generic;
using System.Windows.Forms;
using DsaSoftPanel.FunctionUtility.FunctionConfigView;
using MathNet.Filtering.FIR;
using SeeSharpTools.JY.DSP.Fundamental;
using SeeSharpTools.JY.GUI;

namespace DsaSoftPanel.FunctionUtility
{
    public class FilterFunction : FunctionBase
    {
        public override string ChartXTitle => "Time(ms)";
        public override string ChartYTitle => "Amplitude(V/mV)";
        public override string XValueLabelFormat => "Time: {0:f2}ms";
        public override string YValueLabelFormat => "Amplitude: {0}V/mV";
        public override bool HasDetailedData => false;
        public override bool HasPlotData => true;
        public override Form ConfigForm => _configForm;
        public override string[] DetailParameters => null;
        public override string[] DetailValues { get; protected set; }

        private List<double> _filteredDatas;
//        private double[] _filteredWave;

        private readonly FilterConfigForm _configForm = new FilterConfigForm();

        public FilterFunction(List<double[]> dataBuf) : base(dataBuf)
        {
            _filteredDatas = new List<double>(Constants.DefaultDisplayBufSize);
        }

        protected override void Execute()
        {
            _filteredDatas.Clear();
            OnlineFirFilter filter = _configForm.Filter;
            for (int i = 0; i < this.DataBuf.Count; i++)
            {
                double[] showData = this.DataBuf[i];
                _filteredDatas.AddRange(filter.ProcessSamples(showData));
            }
        }

        protected override void PlotData()
        {
            int channelCount = GlobalInfo.EnableChannelCount;
            double df = 1000.0/GlobalInfo.SampleRate;
            double start = 0;
            GlobalInfo.FunctionPlot.Invoke(_filteredDatas, start, df, _filteredDatas.Count/channelCount);
        }

        public override void RefreshConfigForm()
        {
            return;
        }

        public override void ConfigChart(EasyChartX chart)
        {
            return;
        }
    }
}