using System;
using System.Collections.Generic;

namespace Academits.Karetskas.AtmLogic
{
    public interface IAtmManager
    {
        event Action<AtmState> ChangeAtmState;
        event Action<SafeState> ChangeSafeState;
        event Action<List<Banknote>> AvailableDenominations;
        event Action<List<Banknote>> DispenseMoney;

        AtmState AtmState { get; }

        SafeState SafeState { get; }

        void TurnOn();

        void TurnOff();

        void Fix();

        void TopUpSafe();

        void SetBanknotes(List<Banknote> banknotes);

        void GetBanknotesDenominations(int sum);

        void GetMoney(Banknote banknote, int sum);
    }
}
