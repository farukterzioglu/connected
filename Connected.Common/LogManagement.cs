using System;
using System.Diagnostics;
using Common;

namespace Connected.Common.LogManagement
{
    //Loger sınıfları
    public interface ILoger
    {
        void WriteLog(string text);
        //void WriteLog(string text,int logType);
        void WriteLog(string text, Enums.LogType logType, string source);
    }

    public class DbLoger : ILoger
    {
        public void WriteLog(string text)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                var e = ex;
            }
        }
        public void WriteLog(string text, Enums.LogType logType, string source)
        {
            throw new NotImplementedException();
        }
    }
    public class EventLoger : ILoger
    {
        public void WriteLog(string text)
        {
            throw new NotImplementedException();
        }

        //public void WriteLog(string text, int logType)
        //{
        //    EventLogEntryType eventLogType =
        //        logType == (int)Enums.LogType.Info ? EventLogEntryType.Information : (
        //        logType == (int)Enums.LogType.Error ? EventLogEntryType.Error :
        //        EventLogEntryType.Information);

        //    EventLog mEventLog = new EventLog("Log Manager");
        //    mEventLog.WriteEntry(text, eventLogType);
        //}

        public void WriteLog(string text, Enums.LogType logType, string source)
        {
            EventLogEntryType eventLogType = 
                logType == Enums.LogType.Info? EventLogEntryType.Information : (
                logType == Enums.LogType.Error ? EventLogEntryType.Error : 
                EventLogEntryType.Information);

            EventLog mEventLog = new EventLog(""){ Source = source};
            mEventLog.WriteEntry(text, eventLogType);
        }
    }
    public class TextLoger : ILoger
    {
        public void WriteLog(string text)
        {
            throw new NotImplementedException();
            //TrafficTools.WriteLog.WL(text);
        }

        public void WriteLog(string text, Enums.LogType logType, string source)
        {
            throw new NotImplementedException();
            //TrafficTools.WriteLog.WL("Source : "+ source + ", Type : " + logType.ToString()+ ", Message : " + text);
        }
    }
    #region other Logger implemantations
    //public class XMLLoger : ILoger
    //{
    //    public void WriteLog(string text, int logType)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public void WriteLog(string text, int logType, int? logAltTipi = null, int? userId = null, int? kavsakId = null, int? sensorId = null)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    //public class MailLoger : ILoger
    //{
    //    public void WriteLog(string text, int logType)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void WriteLog(string text, int logType, int? logAltTipi = null, int? userId = null, int? kavsakId = null, int? sensorId = null)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    #endregion    
 
    public class LogManagement
    {
        private ILoger _loger = null;

        public ILoger GetLoger()
        {
            try
            {
                _loger = new DbLoger();
            }
            catch (Exception ex) { var e = ex; }

            return _loger ?? (_loger = new EventLoger());
        }

        public ILoger GetALoger(ILoger loger)
        {
            ILoger aLoger = null;
            try
            {
                aLoger = loger;
            }
            catch (Exception ex) { var e = ex; }

            return aLoger ?? (new EventLoger());
        }
    }
}