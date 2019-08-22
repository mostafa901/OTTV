using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Utility.MVVM;
namespace OTTV.Model
{
    

    public class Item : BaseDataObject
    {

        #region MBE

        private double _MBE;
        //current case by user or from Revit-Ottv/same value from use
        public double MBE
        {
            get
            {
                return _MBE;
            }
            set { SetProperty(ref _MBE, value); }

        }
        #endregion

        #region Ottv
        // ((E5* F5* G5* H5)+(I5* J5* K5*L5* (1-M5)))/N5
        private double _Ottv;

        public double Ottv
        {
            get
            {
                return (alpha*AWi*Ugi*Tdeqwi+Agi*SF*SC*CF*(1-SGR))/Ao;
            }
            set { SetProperty(ref _Ottv, value); }

        }
        #endregion


        #region Tdeqwi

        private double _Tdeqwi;

        public double Tdeqwi
        {
            get
            {
                return _Tdeqwi;
            }
            set { SetProperty(ref _Tdeqwi, value); }

        }
        #endregion


        #region Ugi

        private double _Ugi;

        public double Ugi
        {
            get
            {
                return _Ugi;
            }
            set { SetProperty(ref _Ugi, value); }

        }
        #endregion


        #region AWi

        private double _AWi;

        public double AWi
        {
            get
            {
                return _AWi;
            }
            set { SetProperty(ref _AWi, value); }

        }
        #endregion

        #region Elevation

        private string _Elevation;

        public string Elevation
        {
            get
            {
                return _Elevation;
            }
            set { SetProperty(ref _Elevation, value); }

        }
        #endregion


        #region alpha

        private double _alpha;

        public double alpha
        {
            get
            {
                return _alpha;
            }
            set { SetProperty(ref _alpha, value); }

        }
        #endregion


        #region Agi

        private double _Agi;

        public double Agi
        {
            get
            {
                return _Agi;
            }
            set { SetProperty(ref _Agi, value); }

        }
        #endregion


        #region SF

        private double _SF;

        public double SF
        {
            get
            {
                return _SF;
            }
            set { SetProperty(ref _SF, value); }

        }
        #endregion


        #region SC

        private double _SC;

        public double SC
        {
            get
            {
                return _SC;
            }
            set { SetProperty(ref _SC, value); }

        }
        #endregion


        #region CF

        private double _CF;

        public double CF
        {
            get
            {
                return _CF;
            }
            set { SetProperty(ref _CF, value); }

        }
        #endregion


        #region SGR

        private double _SGR;

        public double SGR
        {
            get
            {
                return _SGR;
            }
            set { SetProperty(ref _SGR, value); }

        }
        #endregion


        #region Ao

        private double _Ao;

        public double Ao
        {
            get
            {
                return _Ao;
            }
            set { SetProperty(ref _Ao, value); }

        }
        #endregion

//equational value 1/ugi
        #region Rvalue

        private double _Rvalue;

        public double Rvalue
        {
            get
            {
                return 1/Ugi;
            }
            set { SetProperty(ref _Rvalue, value); }

        }
        #endregion
         
    }
}
