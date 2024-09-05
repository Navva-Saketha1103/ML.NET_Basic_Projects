using LaptopPricePrediction;
public class Program{
    public static void Main(string[] args)
    {
        start:
            Console.WriteLine("Please enter the following details for better prediction..");
            string CPU = @"i5";
            float GHz = 2.5F;
            string GPU = @"intel";
            float RAM = 4F;
            string RAMType = @"ddr3";
            float Screen = 13.3F;
            float Storage = 500F;
            bool SSD = false;
            float Weight = 2F;

        readValues:
            try
            {
                Console.WriteLine("CPU: ");
                CPU = Console.ReadLine() ?? CPU;
                Console.WriteLine("GHz: ");
                GHz = float.Parse(Console.ReadLine() ?? GHz.ToString());
                Console.WriteLine("GPU: ");
                GPU = Console.ReadLine() ?? GPU;
                Console.WriteLine("RAM: ");
                RAM = float.Parse(Console.ReadLine() ?? RAM.ToString());
                Console.WriteLine("RAMType: ");
                RAMType = Console.ReadLine() ?? RAMType;
                Console.WriteLine("Screen: ");
                Screen = float.Parse(Console.ReadLine() ?? Screen.ToString());
                Console.WriteLine("Storage: ");
                Storage = float.Parse(Console.ReadLine() ?? Storage.ToString());
                Console.WriteLine("SSD: ");
                SSD = bool.Parse(Console.ReadLine() ?? SSD.ToString());
                Console.WriteLine("Weight: ");
                Weight = float.Parse(Console.ReadLine() ?? Weight.ToString());
            }catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            

        var inputData = new LaptopPricePredictor_ML.ModelInput()
            {
                CPU = CPU,
                GHz = GHz,
                GPU = GPU,
                RAM = RAM,
                RAMType = RAMType,
                Screen = Screen,
                Storage = Storage,
                SSD = SSD,
                Weight = Weight,
            };

        var result = LaptopPricePredictor_ML.Predict(inputData);
        Console.Write("Predicted Price: ", result.Score);  
    }
}



