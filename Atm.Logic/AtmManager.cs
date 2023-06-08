using System;
using System.Linq;
using System.Collections.Generic;

namespace Academits.Karetskas.AtmLogic
{
    public sealed class AtmManager : IAtmManager
    {
        private AtmState _atmState;
        private SafeState _safeState;

        private readonly Atm _atm;

        public event Action<AtmState> ChangeAtmState;
        public event Action<SafeState> ChangeSafeState;
        public event Action<List<Banknote>> AvailableDenominations;
        public event Action<List<Banknote>> DispenseMoney;

        public AtmState AtmState { get; }

        public SafeState SafeState { get; }

        public AtmManager()
        {
            _atm = new Atm(100);

            _atmState = AtmState.Off;
            _safeState = SafeState.Empty;
        }

        public void TurnOn()
        {
            if (_atmState == AtmState.Broken || _atmState == AtmState.On)
            {
                return;
            }

            _atmState = AtmState.On;
            ChangeAtmState?.Invoke(_atmState);

            CheckSafeState();
        }

        public void TurnOff()
        {
            if (_atmState == AtmState.Broken || _atmState == AtmState.Off)
            {
                return;
            }

            _atmState = AtmState.Off;
            ChangeAtmState?.Invoke(_atmState);
        }

        public void Fix()
        {
            if (_atmState == AtmState.On || _atmState == AtmState.Off)
            {
                return;
            }

            _atmState = AtmState.Off;
            ChangeAtmState?.Invoke(_atmState);
        }

        public void TopUpSafe()
        {
            if (_atmState != AtmState.Off)
            {
                return;
            }

            var banknotesTypes = (Banknote[])Enum.GetValues(typeof(Banknote));
            var safeHalfCapacity = _atm.SafeCapacity / 2d / 100;
            var banknotesCount = Convert.ToInt32(Math.Round(safeHalfCapacity * (100d / banknotesTypes.Length), MidpointRounding.AwayFromZero));
            var banknotes = new List<Banknote>(banknotesCount);

            foreach (var banknote in banknotesTypes)
            {
                for (var i = 0; i < banknotesCount; i++)
                {
                    banknotes.Add(banknote);
                }

                _atm.SetBanknotes(banknotes);
                banknotes.Clear();
            }
        }

        public void SetBanknotes(List<Banknote> banknotes)
        {
            if (_atmState != AtmState.On || _safeState == SafeState.Full)
            {
                return;
            }

            _atm.SetBanknotes(banknotes);

            CheckSafeState();
        }

        public void GetBanknotesDenominations(int sum)
        {
            if (_atmState != AtmState.On || _safeState == SafeState.Empty)
            {
                AvailableDenominations?.Invoke(null);
                return;
            }

            var banknotesTypes = (Banknote[])Enum.GetValues(typeof(Banknote));
            var banknotesDenominations = new List<Banknote>(banknotesTypes.Length);

            banknotesDenominations
                .AddRange(banknotesTypes.Where(banknoteType => _atm.CheckDispenseEligibility(banknoteType, sum)));

            AvailableDenominations?.Invoke(banknotesDenominations);
        }

        public void GetMoney(Banknote banknote, int sum)
        {
            if (_atmState != AtmState.On || _safeState == SafeState.Empty)
            {
                DispenseMoney?.Invoke(null);
                return;
            }

            var money = _atm.GetMoney(banknote, sum).ToList();

            CheckSafeState();
            DispenseMoney?.Invoke(money);
        }

        private void CheckSafeState()
        {
            var safeCapacityOnePercent = _atm.SafeCapacity / 100d;
            var safeFifteenPercent = (int)Math.Round(15 * safeCapacityOnePercent);
            var safeSeventyFivePercent = (int)Math.Round(75 * safeCapacityOnePercent);

            if (_atm.BanknotesCount < safeFifteenPercent && _atm.BanknotesCount > 0)
            {
                _safeState = SafeState.AlmostEmpty;
            }
            else if (_atm.BanknotesCount > safeSeventyFivePercent && _atm.BanknotesCount < _atm.SafeCapacity)
            {
                _safeState = SafeState.AlmostFull;
            }
            else if (_atm.BanknotesCount >= safeFifteenPercent && _atm.BanknotesCount <= safeSeventyFivePercent)
            {
                _safeState = SafeState.Medium;
            }
            else if (_atm.BanknotesCount == 0)
            {
                _safeState = SafeState.Empty;
            }
            else if (_atm.BanknotesCount == _atm.SafeCapacity)
            {
                _safeState = SafeState.Full;
            }

            ChangeSafeState?.Invoke(_safeState);
        }
    }
}
