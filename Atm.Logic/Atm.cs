using System;
using System.Linq;
using System.Collections.Generic;

namespace Academits.Karetskas.AtmLogic
{
    public sealed class Atm
    {
        private readonly List<Banknote> _safe;

        public int BanknotesCount => _safe.Count;

        public int SafeCapacity { get; }

        public Atm(int safeCapacity)
        {
            if (safeCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(safeCapacity),
                    $"The argument \"{nameof(safeCapacity)}\" = {nameof(safeCapacity)} must be greater than 0.");
            }

            _safe = new List<Banknote>(safeCapacity);
            SafeCapacity = safeCapacity;
        }

        private static void CheckForObjectNull(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), $"The argument {nameof(obj)} is null.");
            }
        }

        private static void CheckArgumentOutOfRange(int minSize, int maxSize, int currentValue)
        {
            if (currentValue < minSize)
            {
                throw new ArgumentOutOfRangeException(nameof(currentValue),
                    $"The argument \"{nameof(currentValue)}\" = {nameof(currentValue)} is less than {nameof(minSize)}.");
            }

            if (currentValue > maxSize)
            {
                throw new ArgumentOutOfRangeException(nameof(currentValue),
                    $"The argument \"{nameof(currentValue)}\" = {nameof(currentValue)} is greater than or equal to {nameof(maxSize)}.");
            }
        }

        public void SetBanknotes(List<Banknote> banknotes)
        {
            CheckForObjectNull(banknotes);
            CheckArgumentOutOfRange(0, SafeCapacity, banknotes.Count + _safe.Count);

            var supportedBanknotes = banknotes.Where(item => item > Banknote.None);
            _safe.AddRange(supportedBanknotes);
        }

        public bool CheckDispenseEligibility(Banknote banknoteType, int sum)
        {
            return GetMoney(banknoteType, sum, (a, b) => { });
        }

        public IEnumerable<Banknote> GetMoney(Banknote banknoteType, int sum)
        {
            var banknotes = new List<Banknote>();

            return GetMoney(banknoteType, sum, (banknote, count) =>
            {
                for (var i = 0; i < count; i++)
                {
                    banknotes.Add(banknote);
                }

                _safe.Where(b => b == banknote)
                    .Take(count)
                    .ToList()
                    .ForEach(b => _safe.Remove(b));
            })
                ? banknotes
                : null;
        }

        private bool GetMoney(Banknote banknoteType, int sum, Action<Banknote, int> task)
        {
            var banknotesCountByType = _safe.Where(banknote => banknote <= banknoteType)
                .GroupBy(banknote => banknote)
                .OrderByDescending(group => group.Key)
                .Select(group => (type: group.Key, count: group.Count()))
                .ToList();

            var banknotesSumByType = banknotesCountByType
                .Sum(banknoteCountByType => banknoteCountByType.count * (int)banknoteCountByType.type);

            if (banknotesSumByType < sum)
            {
                return false;
            }

            var totalSum = sum;

            foreach (var banknote in banknotesCountByType)
            {
                var requiredBanknotesCount = totalSum / (int)banknote.type;

                if (requiredBanknotesCount == 0)
                {
                    continue;
                }

                var subtrahend = requiredBanknotesCount < banknote.count ? requiredBanknotesCount : banknote.count;

                totalSum -= subtrahend * (int)banknote.type;

                task(banknote.type, subtrahend);

                if (totalSum == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
