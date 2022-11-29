namespace PrimeCheckerSocket
{
    class PrimeChecker
    {
        public PrimeChecker(double userNumber) 
        {
            Conclusion(userNumber);
        }

        public static string Conclusion(double userNumber)
        {
            string conclusion;
            bool prime;

            if (userNumber == 0)
            {
                conclusion = "0 is neither prime nor composite.";
            }
            else if (userNumber == 1)
            {
                conclusion = "1 is neither prime nor composite.";
            }
            else
            {
                prime = IsPrime(userNumber);
                if (prime)
                {
                    conclusion = userNumber + " is prime.";
                }
                else
                {
                    conclusion = userNumber + " is not prime.";
                }
            }
            
            return conclusion;
        }

        public static bool IsPrime(double userNumber)
        {
            if (userNumber == 2 || userNumber == 3)
            {

                return true;

            }
            else if (((double)(userNumber - 1) / 6) % 1 == 0)
            {

                return true;

            }
            else if (((double)(userNumber + 1) / 6) % 1 == 0)
            {

                return true;

            }
            else
            {

                return false;
            }
        }
    }
}
