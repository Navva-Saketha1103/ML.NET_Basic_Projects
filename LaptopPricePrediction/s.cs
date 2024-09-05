
using System.Linq;

namespace AdditionOfBinaryFloatValues
{
    public class AddBinaryNumbers
    {
        internal float FloatValue = 0.0f;
        internal string BinaryValue = String.Empty;
        internal int Sign = 0;
        internal int IntegralPlaces = 0;
        internal AddBinaryNumbers() { }
        internal AddBinaryNumbers(float Input)
        {
            this.FloatValue = Input;
            if (this.FloatValue < 0)
            {
                Sign = 1;
                this.FloatValue = -this.FloatValue;
            }
            this.BinaryValue = FloatToBinary(this.FloatValue, ref this.IntegralPlaces);
        }
        static string FloatToBinary(float FloatValue, ref int IntegralPlaces)
        {
            int FloatIntegral = ((int)FloatValue);
            float FloatFractional = (FloatValue - FloatIntegral);
            string BinaryIntegral = IntegralToBinary(FloatIntegral.ToString());
            IntegralPlaces = BinaryIntegral.Length;
            string BinaryFractional = FractionalToBinary(FloatFractional.ToString());
            string BinaryNumber = BinaryIntegral + "." + BinaryFractional;
            return BinaryNumber;
        }
        static string TwosCompliment(string BinaryNumber)
        {
            Console.WriteLine(BinaryNumber);
            int Length = BinaryNumber.Length, Iterator = 0, Point = 0;
            if (Length - 1 == Point)
            {
                BinaryNumber += '0';
            }
            string Compliment = string.Empty, TwosCompliment, AddOne;
            while (Iterator < Length - 1)
            {
                if (BinaryNumber[Iterator] == '0')
                {
                    Compliment += '1';
                }
                else if (BinaryNumber[Iterator] == '1')
                {
                    Compliment += '0';
                }
                else
                {
                    Compliment += ".";
                    Point = Iterator;
                }
                Iterator++;
            }
            Console.WriteLine(AddOne);
            TwosCompliment = AddTwoBinary(Compliment, AddOne, 0);
            Console.WriteLine(TwosCompliment);
            return TwosCompliment;
        }

        static string AddTwoBinary(string Binary1, string Binary2, int Sign)
        {
            int DotOfBinary1 = Binary1.IndexOf('.') + 1, LengthOfBinary1 = Binary1.Length, DotOfBinary2 = Binary2.IndexOf('.') + 1, LengthOfBinary2 = Binary2.Length;
            int FractionalPartOfBinary1 = LengthOfBinary1 - DotOfBinary1;
            int FractionalPartOfBinary2 = LengthOfBinary2 - DotOfBinary2;
            int Carry = 0;

            if (FractionalPartOfBinary1 < FractionalPartOfBinary2)
            {
                Binary1 += string.Concat(Enumerable.Repeat("0", (FractionalPartOfBinary2 - FractionalPartOfBinary1)));
                LengthOfBinary1 = Binary1.Length;
            }
            else if (FractionalPartOfBinary1 > FractionalPartOfBinary2)
            {
                Binary2 += string.Concat(Enumerable.Repeat("0", (FractionalPartOfBinary1 - FractionalPartOfBinary2)));
                LengthOfBinary2 = Binary2.Length;
            }

            int i = LengthOfBinary1 - 1, j = LengthOfBinary2 - 1;
            string SumOfBinary = "";

            while ((i >= 0) && (j >= 0))
            {
                if (Binary1[i] == '1' && Binary2[j] == '1')
                {
                    if (Carry == 0)
                    {
                        SumOfBinary = '0' + SumOfBinary;
                        Carry = 1;
                    }
                    else
                    {
                        SumOfBinary = '1' + SumOfBinary;
                    }
                }
                else if (Binary1[i] == '0' && Binary2[j] == '0')
                {
                    if (Carry == 1)
                    {
                        SumOfBinary = '1' + SumOfBinary;
                        Carry = 0;
                    }
                    else
                    {
                        SumOfBinary = '0' + SumOfBinary;
                    }
                }
                else if ((Binary1[i] == '0' && Binary2[j] == '1') || (Binary1[i] == '1' && Binary2[j] == '0'))
                {
                    if (Carry == 1)
                    {
                        SumOfBinary = '0' + SumOfBinary;
                    }
                    else
                    {
                        SumOfBinary = '1' + SumOfBinary;
                    }
                }
                else
                {
                    SumOfBinary = Binary1[i] + SumOfBinary;
                }
                i--;
                j--;
            }

            while (i >= 0)
            {
                if (Carry == 1)
                {
                    if (Binary1[i] == '1')
                    {
                        SumOfBinary = '0' + SumOfBinary;
                    }
                    else
                    {
                        SumOfBinary = '1' + SumOfBinary;
                        Carry = 0;
                    }
                }
                else
                {
                    SumOfBinary = Binary1[i] + SumOfBinary;
                }
                i--;
            }

            while (j >= 0)
            {
                if (Carry == 1)
                {
                    if (Binary2[j] == '1')
                    {
                        SumOfBinary = '0' + SumOfBinary;
                    }
                    else
                    {
                        SumOfBinary = '1' + SumOfBinary;
                        Carry = 0;
                    }
                }
                else
                {
                    SumOfBinary = Binary2[j] + SumOfBinary;
                }
                j--;
            }

            if (Sign == 0 && Carry == 1)
            {
                SumOfBinary = '1' + SumOfBinary;
            }
            return SumOfBinary;
        }
        static string IntegralToBinary(string IntegralPart)
        {
            string Binary = "";
            int Integral = Convert.ToInt32(IntegralPart);
            int Remainder;
            while (Integral != 0)
            {
                Remainder = Integral % 2;
                if (Remainder == 1)
                {
                    Binary = '1' + Binary;
                }
                else
                {
                    Binary = '0' + Binary;
                }
                Integral = Integral / 2;
            }
            return Binary;
        }

        static string FractionalToBinary(string FractionalPart)
        {
            string Binary = "";
            float Fractional = float.Parse(FractionalPart);
            while (Fractional != 0)
            {
                Fractional = Fractional * 2;

                if (Fractional >= 1)
                {
                    Binary = Binary + '1';
                    Fractional--;
                }
                else
                {
                    Binary = Binary + '0';
                }
            }
            return Binary;
        }

        static int BIntegralToFloat(string BinaryIntegral, int Length, int IntegralBits, int Integral)
        {
            if (Length < 0)
            {
                return Integral;
            }
            if (BinaryIntegral[Length] == '1')
            {
                Integral += IntegralBits;
            }
            return BIntegralToFloat(BinaryIntegral, Length - 1, IntegralBits * 2, Integral);
        }

        static float BFractionalToFloat(string BinaryFractional, int Length, float Bits, float Fractional, int Iterator)
        {
            if (Iterator >= Length)
            {
                return Fractional;
            }
            if (BinaryFractional[Iterator] == '1')
            {
                Fractional += Bits;
            }
            return BFractionalToFloat(BinaryFractional, Length, Bits * 0.5f, Fractional, Iterator + 1);
        }

        static float BinaryToFloat(string Binary)
        {
            int IntegralBits = 1, Integral = 0, FractionalBits = 0;
            string BinaryIntegral = Binary.Substring(0, Binary.IndexOf('.'));
            string BinaryFractional = Binary.Substring(Binary.IndexOf('.') + 1);
            Integral = BIntegralToFloat(BinaryIntegral, BinaryIntegral.Length - 1, IntegralBits, Integral);
            float Fractional = BFractionalToFloat(BinaryFractional, BinaryFractional.Length, 0.5f, FractionalBits, 0);
            float FloatValue = Integral + Fractional;
            return FloatValue;
        }

        public static AddBinaryNumbers operator +(AddBinaryNumbers Binary1, AddBinaryNumbers Binary2)
        {
            AddBinaryNumbers Binary3 = new AddBinaryNumbers();
            if (Binary1.FloatValue <= 0)
            {
                Binary1.FloatValue = -Binary1.FloatValue;
            }
            int LengthOfBinary1 = Binary1.BinaryValue.Length;
            if (Binary1.BinaryValue[LengthOfBinary1 - 1] == '.')
            {
                Binary1.BinaryValue += '1';
            }
            int LengthOfBinary2 = Binary2.BinaryValue.Length;
            if (Binary2.BinaryValue[LengthOfBinary2 - 1] == '.')
            {
                Binary2.BinaryValue += '1';
            }

            if (Binary1.Sign != Binary2.Sign)
            {

                Binary3.Sign = 1;
                int IntegralOfBinary1 = Binary1.BinaryValue.IndexOf('.');
                int IntegralOfBinary2 = Binary2.BinaryValue.IndexOf('.');

                if (Binary1.FloatValue >= Binary2.FloatValue)
                {
                    Binary2.BinaryValue = string.Concat(Enumerable.Repeat("0", IntegralOfBinary1 - IntegralOfBinary2)) + Binary2.BinaryValue;
                    Binary2.BinaryValue = TwosCompliment(Binary2.BinaryValue);
                }
                else
                {
                    Binary1.BinaryValue = string.Concat(Enumerable.Repeat("0", IntegralOfBinary2 - IntegralOfBinary2)) + Binary1.BinaryValue;
                    Binary1.BinaryValue = TwosCompliment(Binary1.BinaryValue);
                }
                Binary3.BinaryValue = AddTwoBinary(Binary1.BinaryValue, Binary2.BinaryValue, Binary3.Sign);

                Console.WriteLine(Binary3.BinaryValue);
                Console.WriteLine(Binary2.BinaryValue);
                Console.WriteLine(Binary1.BinaryValue);
                Binary3.FloatValue = BinaryToFloat(Binary3.BinaryValue);
            }
            else
            {
                Binary3.BinaryValue = AddTwoBinary(Binary1.BinaryValue, Binary2.BinaryValue, Binary3.Sign);
                Binary3.FloatValue = BinaryToFloat(Binary3.BinaryValue);
            }

            if (Binary1.Sign == 1 && Binary1.Sign == 1)
            {
                Binary3.FloatValue = -Binary3.FloatValue;
            }
            else if ((Binary1.Sign == 0 && Binary2.Sign == 1) && (Binary1.FloatValue < Binary2.FloatValue))
            {
                Binary3.FloatValue = -Binary3.FloatValue;
            }
            else if ((Binary1.Sign == 1 && Binary2.Sign == 0) && (Binary1.FloatValue > Binary2.FloatValue))
            {
                Binary3.FloatValue = -Binary3.FloatValue;
            }

            if (Binary3.FloatValue >= 0)
            {
                Binary3.Sign = 0;
            }
            else
            {
                Binary3.Sign = 1;
            }
            //  if(Binary3.FloatValue == -0)
            //{
            //  Binary3.FloatValue = 0.0f;
            //}
            return Binary3;
        }
    }



    class Program
    {

        public static void Main(string[] args)
        {
        start:
            Console.WriteLine("Please Enter 2 floating point values: (e.g: 1.23)");
            float Input1 = 0.0f;
            float Input2 = 0.0f;
            float Input3 = 0.0f;
            AddBinaryNumbers add = new AddBinaryNumbers();

        readValues:
            try
            {
                Console.WriteLine("Enter first value");
                Input1 = float.Parse(Console.ReadLine());
                AddBinaryNumbers binary1 = new AddBinaryNumbers(Input1);

                Console.WriteLine("Enter Second value");
                Input2 = float.Parse(Console.ReadLine());
                AddBinaryNumbers binary2 = new AddBinaryNumbers(Input2);

                Console.WriteLine("Enter Third value");
                Input3 = float.Parse(Console.ReadLine());
                AddBinaryNumbers binary3 = new AddBinaryNumbers(Input3);

                AddBinaryNumbers binary = new AddBinaryNumbers();
                binary = binary1 + binary2 + binary3;
                binary.FloatValue = (float)Math.Round(binary.FloatValue * 100f) / 100f;
                Console.WriteLine("Sum of given float values is: " + binary.FloatValue);
            }
            catch (Exception)
            {
                Console.WriteLine("Please Enter Numeric Values!!!");
                goto readValues;
            }

            Console.WriteLine("Want to add one more pair of float values?");
            Console.WriteLine("Press 1 to continue");
            Console.WriteLine("Pres any key to Exit(0)");
            if (Console.ReadLine() == "1")
            {
                goto start;
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}