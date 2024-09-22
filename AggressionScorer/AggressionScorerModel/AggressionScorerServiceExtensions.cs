using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;

namespace AggressionScorerModel
{
    public static class AggressionScorerServiceExtensions
    {
        private static readonly string _modelfile = @"C:\\Users\\navva\\OneDrive\\Desktop\\ML.NET_Basic_Projects\\AggressionScorer\\AggressionScorerModel\\Model\\AggressionScoreModel.zip";

        public static void AddAggressionScorePredictionEnginePool(this IServiceCollection services)
        {
            services.AddPredictionEnginePool<UserInput, UserOutput>()
                .FromFile(filePath: _modelfile, watchForChanges: true);
        }
    }
}
