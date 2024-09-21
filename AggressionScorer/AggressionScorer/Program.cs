using AggressionScorerModel;
using Microsoft.ML;

namespace AggressionScorer
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Aggression Scorer");

            var mlContext = new MLContext(0);

            // Build Pipeline
            var inputDataPreparer = mlContext
                                        .Transforms
                                        .Text
                                        .FeaturizeText("Features", "Comment")
                                        .AppendCacheCheckpoint(mlContext);

            var trainer = mlContext
                            .BinaryClassification
                            .Trainers
                            .LbfgsLogisticRegression();

            var trainingPipeline = inputDataPreparer.Append(trainer);

            // Load data
            var createdInputFile = @"C:\Users\navva\OneDrive\Desktop\ML.NET_Basic_Projects\AggressionScorer\AggressionScorer\Data\preparedInput.tsv";
            DataPreparer.CreatePreparedDataFile(createdInputFile, onlySaveSmallSubset: true);

            IDataView trainDataView = mlContext.Data.LoadFromTextFile<UserInput>(
                                            path: createdInputFile,
                                            hasHeader: true,
                                            separatorChar: '\t',
                                            allowQuoting: true
                                        );

            // Fit the model
            ITransformer model = trainingPipeline.Fit(trainDataView);

            // Test the model


            // Save the model
            var modelFile = @"C:\\Users\\navva\\OneDrive\\Desktop\\ML.NET_Basic_Projects\\AggressionScorer\\AggressionScorer\\Model\\AggressionScoreModel.zip";
            mlContext.Model.Save(model, trainDataView.Schema, modelFile);
            Console.WriteLine(modelFile);
        }
    }
}
