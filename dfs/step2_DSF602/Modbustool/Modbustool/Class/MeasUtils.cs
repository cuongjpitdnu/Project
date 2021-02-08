// Modbustool.MeasUtils
using Modbustool;
using System;

internal static class MeasUtils
{
    public enum TH_KIND
    {
        TH_ELE_NONE,
        TH_ELE_THERMISTOR,
        TH_ELE_PT1000
    }

    public struct TPAD
    {
        public int mv;

        public int mv_100;

        public TH_KIND th;
    }

    public const ulong MEAS_AD_REF_VOLT = 3300uL;

    public const ulong MEAS_AD_RESOLUTION = 16777216uL;

    private const int _TEMP_CAL_MIN = 0;

    private const int _TEMP_CAL_MAX = 1000;

    public const double TEMP_PARALLEL_R = 20000.0;

    public const int TEMP_DISP_MIN_C = -50;

    public const int TEMP_DISP_MAX_C = 1100;

    public const int TEMP_OVER_VAL_C = 2000;

    public const int TEMP_CAL_RANGE_C = 50;

    public static uint ADtomv(uint ad_data)
    {
        ulong value = (ulong)(330000L * (long)ConvertLinerScale(ad_data)) / 16777216uL;
        return Convert.ToUInt32(value);
    }

    private static uint ConvertLinerScale(uint ad_data)
    {
        if (((int)ad_data & -268435456) == 805306368)
        {
            return 16777215u;
        }
        if (((int)ad_data & -268435456) == 0)
        {
            return 0u;
        }
        if ((ad_data & 0x20000000) == 536870912)
        {
            return ((ad_data >> 5) & 0x7FFFFF) + 8388608;
        }
        return (ad_data >> 5) & 0x7FFFFF;
    }

    private static int Th_adconv(ref TPAD tp, uint ad, float adj_s, float adj_k)
    {
        double num = 0.0;
        int num2 = 0;
        double num3 = 20000.0;
        double num4 = ad;
        num4 *= 1E-05;
        double num5 = num4 * 20000.0 / (3.3 - num4);
        num5 = (double)adj_s * num5 + (double)adj_k;
        num4 = (3.3 - num4) / 20000.0 * num5;
        double num6 = num3 * num5 / (num3 - num5);
        num6 *= 0.001;
        tp.th = TH_KIND.TH_ELE_THERMISTOR;
        if (num4 >= 1.0)
        {
            tp.th = TH_KIND.TH_ELE_NONE;
            tp.mv = -99;
            tp.mv_100 = tp.mv * 10;
            num2 = -1;
        }
        else if (num6 > 11.1)
        {
            num = 53.416 - 126.95 * num4 + 93.247 * fpow(num4, 2) - 35.91 * fpow(num4, 3);
        }
        else if (num6 >= 4.3)
        {
            num = 53.416 - 126.95 * num4 + 93.247 * fpow(num4, 2) - 35.91 * fpow(num4, 3);
        }
        else if (num6 >= 2.1)
        {
            num = 69.133 - 225.07 * num4 + 300.18 * fpow(num4, 2) - 183.22 * fpow(num4, 3);
        }
        else if (num6 >= 1.053)
        {
            num = 87.085 - 420.53 * num4 + 1019.8 * fpow(num4, 2) - 1078.0 * fpow(num4, 3);
        }
        else if (num6 >= 0.5838)
        {
            num = 106.9 - 802.14 * num4 + 3505.2 * fpow(num4, 2) - 6544.4 * fpow(num4, 3);
        }
        else if (num6 >= 0.3405)
        {
            num = 128.17 - 1520.1 * num4 + 11697.0 * fpow(num4, 2) - 38115.0 * fpow(num4, 3);
        }
        else if (num6 >= 0.2078)
        {
            num = 148.94 - 2693.5 * num4 + 34040.0 * fpow(num4, 2) - 181459.0 * fpow(num4, 3);
        }
        else
        {
            num = 168.94 - 4484.1 * num4 + 88096.0 * fpow(num4, 2) - 731484.0 * fpow(num4, 3);
            if (num * 10.0 > 1100.0)
            {
                tp.th = TH_KIND.TH_ELE_NONE;
                tp.mv = 1101;
                tp.mv_100 = tp.mv * 10;
                num2 = -2;
            }
        }
        if (num2 == 0)
        {
            if (num < 0.0)
            {
                tp.mv = (int)((num - 0.05) * 10.0);
                tp.mv_100 = (int)((num - 0.005) * 100.0);
            }
            else
            {
                tp.mv = (int)((num + 0.05) * 10.0);
                tp.mv_100 = (int)((num + 0.005) * 100.0);
            }
        }
        return num2;
    }

    public static bool Tempreture(ref int Meas_Temp, ref int Meas_Temp_100, ref TPAD tp, uint TpMv, float adj_s, float adj_k, int Corrt, int TestModeFlag)
    {
        int num = Th_adconv(ref tp, TpMv, adj_s, adj_k);
        int num2 = (Math.Abs(Corrt) <= 50 && num == 0) ? Corrt : 0;
        Meas_Temp = tp.mv + num2;
        Meas_Temp_100 = tp.mv_100 + num2 * 10;
        if (Meas_Temp > 1100)
        {
            Meas_Temp = 1100;
            Meas_Temp_100 = Meas_Temp * 10;
        }
        else if (Meas_Temp < -50)
        {
            Meas_Temp = -50;
            Meas_Temp_100 = Meas_Temp * 10;
        }
        bool flag = true;
        if (!flag && TestModeFlag == 0)
        {
            Meas_Temp = 2000;
            Meas_Temp_100 = Meas_Temp * 10;
        }
        return flag;
    }

    public static int TempCal(TPAD TpAd, int var)
    {
        int result = 0;
        if (TpAd.mv >= 0 && TpAd.mv <= 1000)
        {
            int num = var - TpAd.mv;
            if (Math.Abs(num) <= 50)
            {
                result = num;
            }
        }
        return result;
    }

    private static double fpow(double _base, ushort n)
    {
        double num = 1.0;
        for (ushort num2 = 1; num2 <= n; num2 = (ushort)(num2 + 1))
        {
            num *= _base;
        }
        return num;
    }

    public static int Eh_Calc(int _mv, int _temp)
    {
        int num = 12;
        int[] array = new int[12]
        {
            0,
            50,
            100,
            150,
            200,
            250,
            300,
            350,
            400,
            450,
            500,
            550
        };
        int[] array2 = new int[12]
        {
            2240,
            2210,
            2170,
            2140,
            2100,
            2060,
            2030,
            1990,
            1960,
            1920,
            1880,
            1850
        };
        int i;
        if (_temp <= array[0])
        {
            i = 0;
        }
        else if (_temp >= array[num - 1])
        {
            i = num - 2;
        }
        else
        {
            for (i = 0; i < num - 1 && (array[i] > _temp || array[i + 1] <= _temp); i++)
            {
            }
        }
        double num2 = (double)(array2[i + 1] - array2[i]) / (double)(array[i + 1] - array[i]);
        double num3 = (num2 * (double)_temp + (double)array2[i + 1] - num2 * (double)array[i + 1]) * 10.0;
        int num4 = (!(num3 < 0.0)) ? ((int)(num3 + 5.0) / 10) : ((int)(num3 - 5.0) / 10);
        return _mv + num4;
    }

    public static int _bitcount(uint x)
    {
        int num = 0;
        num = 0;
        while (x != 0)
        {
            if ((x & 1) == 1)
            {
                num++;
            }
            x >>= 1;
        }
        return num;
    }

    public static int L_Round(int input, int round)
    {
        switch (round)
        {
            case 4:
                {
                    int num = (input >= 0) ? 5000 : (-5000);
                    return (input + num) / 10000;
                }
            case 3:
                {
                    int num = (input >= 0) ? 500 : (-500);
                    return (input + num) / 1000;
                }
            case 2:
                {
                    int num = (input >= 0) ? 50 : (-50);
                    return (input + num) / 100;
                }
            case 1:
                {
                    int num = (input >= 0) ? 5 : (-5);
                    return (input + num) / 10;
                }
            default:
                return input;
        }
    }
}
