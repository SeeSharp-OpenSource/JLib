﻿using System;
using JYPCI69527L;
using DsaSoftPanel.Enumeration;
using Coupling = DsaSoftPanel.Enumeration.Coupling;

namespace DsaSoftPanel.AITask
{
    public class JYPCI69527LAITaskImpl : AITask
    {
        public JYPCI69527LAITask AITask { get; private set; }

        public JYPCI69527LAITaskImpl(int boardId)
        {
            AITask = new JYPCI69527LAITask(boardId);
            AITask.Mode = AIMode.Continuous;
            AITask.Trigger.Type = AITriggerType.Immediate;
        }

        public double MaxSampleRate => 200000;

        public int GetChannelCount()
        {
            return 2;
        }

        public void SetSampleRate(double sampleRate)
        {
            AITask.SampleRate = sampleRate;
        }

        public double GetSampleRate()
        {
            return AITask.SampleRate;
        }

        public void SetSamplesToAcquire(int samplesToAcquire)
        {
            AITask.SamplesToAcquire = samplesToAcquire;
        }

        public int GetSamplesToAcquire()
        {
            return AITask.SamplesToAcquire;
        }

        public void AddChannel(int channelId, double range, Coupling coupling)
        {
            JYPCI69527L.Coupling realCoupling = Coupling.AC == coupling ? JYPCI69527L.Coupling.AC : JYPCI69527L.Coupling.DC;
            AITask.AddChannel(channelId, -1 * range, range, realCoupling);
        }

        public void Start()
        {
            AITask.Start();
        }

        public void ReadData(ref double[] buf, int timeOut)
        {
            AITask.ReadData(ref buf, timeOut);
        }

        public void ReadData(ref double[] buf, int samplesPerChannel, int timeOut)
        {
            AITask.ReadData(ref buf, samplesPerChannel, timeOut);
        }

        public void ReadData(ref double[,] buf, int timeOut = -1)
        {
            AITask.ReadData(ref buf, timeOut);
        }

        public void ReadData(ref double[,] buf, int samplesPerChannel, int timeOut)
        {
            AITask.ReadData(ref buf, samplesPerChannel, timeOut);
        }

        public void Stop()
        {
            AITask.Stop();
        }

        public void ClearChannels()
        {
            AITask.Channels.Clear();
        }

        public bool IsBoardDisconnected(int errorCode)
        {
            throw new System.NotImplementedException();
        }

        public bool TriggerEnabled => AITask.Trigger.Type != AITriggerType.Immediate;

        public string TriggerMode
        {
            get { return AITask.Trigger.Mode.ToString(); }
            set { return; }
        }

        public string TriggerType
        {
            get { return AITask.Trigger.Type.ToString(); }
            set { AITask.Trigger.Type = (AITriggerType) Enum.Parse(typeof(AITriggerType), value); }
        }

        public string AnalogTriggerSource
        {
            get { return AITask.Trigger.Analog.Source.ToString(); }
            set { AITask.Trigger.Analog.Source = (AIAnalogTriggerSource)Enum.Parse(typeof(AIAnalogTriggerSource), value); }
        }

        public string AnalogTriggerCondition
        {
            get { return AITask.Trigger.Analog.Condition.ToString(); }
            set { AITask.Trigger.Analog.Condition = (AIAnalogTriggerCondition)Enum.Parse(typeof(AIAnalogTriggerCondition), value); }
        }

        public double AnalogTriggerThreshold
        {
            get { return AITask.Trigger.Analog.Threshold; }
            set { AITask.Trigger.Analog.Threshold = value; }
        }

        public string DigitalTriggerSource
        {
            get { return AITask.Trigger.Digital.Source.ToString(); }
            set
            {
                AITask.Trigger.Digital.Source = (AIDigitalTriggerSource) Enum.Parse(typeof (AIDigitalTriggerSource), value);
            }
        }

        public string DigitalTriggerEdge
        {
            get { return AITask.Trigger.Digital.Edge.ToString(); }
            set
            {
                AITask.Trigger.Digital.Edge = (AIDigitalTriggerEdge)Enum.Parse(typeof(AIDigitalTriggerEdge), value);
            }
        }

        public string[] GetTriggerModes()
        {
            return Enum.GetNames(typeof (AITriggerMode));
        }

        public string[] GetTriggerTypes()
        {
            return Enum.GetNames(typeof (AITriggerType));
        }

        public string[] GetAnalogTriggerSources()
        {
            return Enum.GetNames(typeof (AIAnalogTriggerSource));
        }

        public string[] GetAnalogTriggerConditions()
        {
            return Enum.GetNames(typeof (AIAnalogTriggerCondition));
        }

        public string[] GetDigitalTriggerSources()
        {
            return Enum.GetNames(typeof (AIDigitalTriggerSource));
        }

        public string[] GetDigitalTriggerEdge()
        {
            return Enum.GetNames(typeof(AIDigitalTriggerEdge));
        }

        public void SendSoftTrigger()
        {
            AITask.SendSoftTrigger();
        }
    }
}