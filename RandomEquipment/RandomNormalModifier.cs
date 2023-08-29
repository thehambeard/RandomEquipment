using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using WrathRandomEquipment.Utility;

namespace WrathRandomEquipment.RandomEquipment
{
    internal class RandomNormalModifier
    {
        public int CurrentModifier { get; private set; }

        private int _minModifier;
        private int _maxModifier;
        private double _shiftCurve;
        private double _scaleCurve;
        private double _minMod;
        private double _maxMod;
        private double _max;
        private double _min;
        private double _mean;
        private double _std;
        private Normal _normal;

        public RandomNormalModifier(int minModifier, int maxModifier, double shiftCurve = 0, double scaleCurve = 1d)
        {
            _minModifier = minModifier < maxModifier ? minModifier : maxModifier;
            _maxModifier = maxModifier > minModifier ? maxModifier : minModifier;
            _shiftCurve = shiftCurve;
            _scaleCurve = scaleCurve;
            Update();
        }

        private void Update()
        {
            _minMod = _minModifier * _scaleCurve + _shiftCurve;
            _maxMod = _maxModifier * _scaleCurve + _shiftCurve;
            _max = (_maxMod + -_minMod);
            _min = 0;
            _mean = (_min + (_max - _min)) / 2;
            _std = (_mean - _min) / 3;
            _normal = new(_mean, _std);
        }

        public int GetModifier(bool updateCurrent = true)
        {
            var result = (int)Math.Round(_normal.Sample() - -_minMod).Clamp(_minModifier, _maxModifier);

            if (updateCurrent)
                CurrentModifier = result;

            return result;
        }

        public Dictionary<int, double> GetDistribution(int accuracy)
        {
            var result = new Dictionary<int, double>();
            for (int i = _minModifier; i <= _maxModifier; i++)
                result.Add(i, 0);

            for (int i = 0; i < accuracy; i++)
                result[GetModifier(false)]++;

            for (int i = _minModifier; i <= _maxModifier; i++)
                result[i] = (result[i] / (double)accuracy) * 100;

            return result;
        }

        public int MinModifier
        {
            get { return _minModifier; }
            set
            {
                _minModifier = value;
                Update();
            }
        }
        public int MaxModifier
        {
            get { return _maxModifier; }
            set
            {
                _maxModifier = value;
                Update();
            }
        }
        public double ShiftCurve
        {
            get { return _shiftCurve; }
            set
            {
                _shiftCurve = value;
                Update();
            }
        }
        public double ScaleCurve
        {
            get { return _scaleCurve; }
            set
            {
                _scaleCurve = value;
                Update();
            }
        }

    }
}
