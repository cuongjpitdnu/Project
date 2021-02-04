using GPConst;
using GPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GPMain.Common.Database
{
    public struct StatisticsData {
        public int Total { get; set; }
        public int Number { get; set; }
        public float Precent => Total != 0 ? (float)Number / Total * 100 : 0;

        public string ToFomat(string fomat = "{0} người ({1: 0.00 })")
        {
            return string.Format(fomat, Number, Precent);
        }
    }

    public class Statistics : IDisposable
    {

        private List<TMember> listMember;

        public Statistics(Expression<Func<TMember, bool>> condition)
        {
            ReLoadData(condition);
        }

        public Statistics(List<TMember> lstMember = null)
        {
            ReLoadData(lstMember);
        }

        public void ReLoadData(List<TMember> lstMember)
        {
            listMember = lstMember ?? AppManager.DBManager.GetTable<TMember>().ToList();
        }

        public void ReLoadData(Expression<Func<TMember, bool>> condition)
        {
            listMember = AppManager.DBManager.GetTable<TMember>().ToList(condition);
        }

        public StatisticsData TotalMember => new StatisticsData
        {
            Number = listMember.Count,
            Total = listMember.Count,
        };

        public StatisticsData TotalGenderMember(EmGender gender) => new StatisticsData
        {
            Number = listMember.Where(i => i.Gender == (int)gender).Count(),
            Total = listMember.Count,
        };

        public StatisticsData TotalStatusMember(bool isAlive) => new StatisticsData
        {
            Number = listMember.Where(i => i.IsDeath == !isAlive).Count(),
            Total = listMember.Count,
        };

        public StatisticsData TotalBettweenAge(int min , int max) => new StatisticsData
        {
            Number = listMember.Where(
                i=> i.Birthday.YearSun > 0 
                && min <= (DateTime.Now.Year - i.Birthday.YearSun)
                && ((max > 0) ? max >= (DateTime.Now.Year - i.Birthday.YearSun) : true)
            ).Count(),
            Total = listMember.Count,
        };


        public List<TMember> getDataByCondition()
        {
            var lstData = listMember;
            return lstData;
        }

        #region Dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~StatisticsData()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Dispose
    }
}
