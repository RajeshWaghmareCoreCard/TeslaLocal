namespace CoreCard.Tesla.Common
{
    public static class CardExtensions
    {
        public static string GetCardBin(this string cardNumber)
        {
            if (cardNumber?.Length >= 6)
            {
                return cardNumber.Substring(0, 6);
            }
            throw new System.Exception("Card length is mismatch");
        }
        public static string GetCardLast4(this string cardNumber)
        {
            if (cardNumber != null && cardNumber.Length >= 4)
            {
                return cardNumber.Substring(cardNumber.Length - 4);
            }
            throw new System.Exception("Card length is mismatch");
        }

        public static string GetHash(this string cardNumber, string salt)
        {
            if (cardNumber != null)
            {
                return cardNumber.HMACSHA512Hash(salt);
            }
            throw new System.Exception("Card length is mismatch");
        }
    }
}