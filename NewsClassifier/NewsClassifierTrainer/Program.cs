using Common;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using NewsClassifierModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NewsClassifierTrainer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("News classification trainer started");
            //FindTheBestModel();  
            //|     Trainer                              MicroAccuracy  MacroAccuracy  Duration                                |
            //| 1    AveragedPerceptronOva                       0.9496         0.6305      43.2 |
            //| 2    SdcaMaximumEntropyMulti                     0.9347         0.6191      24.8
            //| 3    LightGbmMulti                               0.9086         0.5968     313.0 |
            //Results:
            //Micro Accuracy: 0.95
            //Macro Accuracy: 0.63
            //
            //TrainTheModel();

            TrainTheModelWithUnbalacedData();
        }

        private static void TrainTheModelWithUnbalacedData()
        {
            Console.WriteLine("New Classifier Model Trainer");

            var mlContext = new MLContext(seed: 0);

            var trainDataPath = "Data\\uci-news-aggregator.csv";

            var unbalancedDataPath = "Data\\unbalanced.csv";

            CreateUnbalancedDataFile(trainDataPath, unbalancedDataPath);

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                                                                                        unbalancedDataPath,
                                                                                        hasHeader: true,
                                                                                        separatorChar: ',',
                                                                                        allowQuoting: true
                                                                                    );
            var preProcessingPipeline = mlContext.Transforms.Conversion
                                    .MapValueToKey(inputColumnName: "Category", outputColumnName: "Label")
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Title",
                                    outputColumnName: "Features"))
                                    .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                                    .AppendCacheCheckpoint(mlContext);

            var trainer = mlContext.MulticlassClassification.Trainers.OneVersusAll(
                                                mlContext.BinaryClassification.Trainers.AveragedPerceptron()
                                            );


            var trainingPipeline = preProcessingPipeline.Append(trainer)
                                                        .Append(mlContext.Transforms
                                                                         .Conversion
                                                                         .MapKeyToValue("PredictedLabel"
                                                                         )
                                                        );

            var cvResults = mlContext.MulticlassClassification.CrossValidate(trainingDataView, trainingPipeline);

            var microAcc = cvResults.Average(m => m.Metrics.MicroAccuracy);
            var macroAcc = cvResults.Average(m => m.Metrics.MacroAccuracy);
            var logLossReduction = cvResults.Average(m => m.Metrics.LogLossReduction);

            Console.WriteLine(microAcc);
            Console.WriteLine(macroAcc);
            Console.WriteLine(logLossReduction);

            var startTime = DateTime.Now;

            var finalModel = trainingPipeline.Fit(trainingDataView);

            Console.WriteLine(startTime - DateTime.Now);

            if (!(Directory.Exists("Model")))
            {
                Directory.CreateDirectory("Model");
            }

            var modelPath = "Model\\NewClassificationModel.zip";
            mlContext.Model.Save(finalModel, trainingDataView.Schema, modelPath);
            Console.WriteLine("Done");
        }


        private static void TrainTheModel()
        {
            Console.WriteLine("Model Trainer");

            var mlContext = new MLContext(seed: 0);

            var trainDataPath = "Data\\uci-news-aggregator.csv";

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                                                                                        trainDataPath,
                                                                                        hasHeader: true,
                                                                                        separatorChar: ',',
                                                                                        allowQuoting: true
                                                                                    );
            var preProcessingPipeline = mlContext.Transforms.Conversion
                                    .MapValueToKey(inputColumnName: "Category", outputColumnName: "Label")
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Title",
                                    outputColumnName: "Features"))
                                    .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                                    .AppendCacheCheckpoint(mlContext);

            var trainer = mlContext.MulticlassClassification.Trainers.OneVersusAll(
                                                mlContext.BinaryClassification.Trainers.AveragedPerceptron()
                                            );


            var trainingPipeline = preProcessingPipeline.Append(trainer)
                                                        .Append(mlContext.Transforms
                                                                         .Conversion
                                                                         .MapKeyToValue("PredictedLabel"
                                                                         )
                                                        );

            var cvResults = mlContext.MulticlassClassification.CrossValidate(trainingDataView, trainingPipeline);

            var microAcc = cvResults.Average(m => m.Metrics.MicroAccuracy);
            var macroAcc = cvResults.Average(m => m.Metrics.MacroAccuracy);
            var logLossReduction = cvResults.Average(m => m.Metrics.LogLossReduction);

            Console.WriteLine(microAcc);
            Console.WriteLine(macroAcc);
            Console.WriteLine(logLossReduction);

            var startTime = DateTime.Now;

            var finalModel = trainingPipeline.Fit(trainingDataView);

            Console.WriteLine(startTime - DateTime.Now);

            if (!(Directory.Exists("Model")))
            {
                Directory.CreateDirectory("Model");
            }

            var modelPath = "Model\\NewClassificationModel.zip";
            mlContext.Model.Save(finalModel, trainingDataView.Schema, modelPath);
            Console.WriteLine("Done");


        }

        private static void FindTheBestModel()
        {
            Console.WriteLine("Finding the best model using AutoML");

            var mlContext = new MLContext(seed: 0);

            var trainDataPath = "Data\\uci-news-aggregator.csv";

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                                                                                        trainDataPath,
                                                                                        hasHeader: true,
                                                                                        separatorChar: ',',
                                                                                        allowQuoting: true
                                                                                    );

            var preProcessingPipeline = mlContext.Transforms.Conversion
                                            .MapValueToKey(inputColumnName: "Category", outputColumnName: "Category");


            var mappedInputData = preProcessingPipeline.Fit(trainingDataView).Transform(trainingDataView);

            var experimentSettings = new MulticlassExperimentSettings
            {
                MaxExperimentTimeInSeconds = 300,
                CacheBeforeTrainer = CacheBeforeTrainer.On,
                OptimizingMetric = MulticlassClassificationMetric.MicroAccuracy,
                CacheDirectory = null
            };

            var experiment =
                mlContext.Auto().CreateMulticlassClassificationExperiment(experimentSettings);

            Console.WriteLine("Starting Experiment...");

            var expResults =
                experiment.Execute(
                        trainData: mappedInputData,
                        labelColumnName: "Category",
                        progressHandler: new MulticlassExperimentProgressHandler()
                    );

            Console.WriteLine("Results: ");

            var metrics = expResults.BestRun.ValidationMetrics;

            Console.WriteLine($"Micro Accuracy: {metrics.MicroAccuracy:0.##}");
            Console.WriteLine($"Macro Accuracy: {metrics.MacroAccuracy:0.##}");
        }

        private static void CreateUnbalancedDataFile(string inputDataFile, string outputDataFile)
        {
            var inputFileRows = File.ReadAllLines(inputDataFile);
            var outputRows = new List<string>();

            //Add header to output
            outputRows.Add(inputFileRows.First());

            int entertainmentSamples = 0;
            int businessSamples = 0;
            int technologySamples = 0;
            int medicineSamples = 0;

            var randomGenerator = new Random(0);

            foreach(var row in inputFileRows.Skip(1)) { 
                if (row.Contains(",b,"))
                {
                    //Only add it 10% of times
                    if (randomGenerator.NextDouble() <= .1)
                    {
                        outputRows.Add(row);
                        businessSamples++;
                    }
                }
                if (row.Contains(",e,"))
                {
                    //Only add it 10% of times
                    if (randomGenerator.NextDouble() <= .1)
                    {
                        outputRows.Add(row);
                        entertainmentSamples++;
                    }
                }
                if (row.Contains(",t,"))
                {
                    //Only add it 10% of times
                    //if (randomGenerator.NextDouble() <= .1)
                    //{
                        outputRows.Add(row);
                        technologySamples++;
                    //}
                }
                if (row.Contains(",m,"))
                {
                    //Only add it 10% of times
                    //if (randomGenerator.NextDouble() <= .1)
                    //{
                        outputRows.Add(row);
                        medicineSamples++;
                    //}
                }
            }

            File.WriteAllLines(outputDataFile, outputRows);

            Console.WriteLine($"Entertainment: {entertainmentSamples}");
            Console.WriteLine($"Business: {businessSamples}");
            Console.WriteLine($"Technology: {technologySamples}");
            Console.WriteLine($"Medicine: {medicineSamples}");
        }
    }
}
