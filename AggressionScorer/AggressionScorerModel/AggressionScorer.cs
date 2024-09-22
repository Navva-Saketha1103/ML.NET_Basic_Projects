using Microsoft.Extensions.ML;

namespace AggressionScorerModel
{
    public class AggressionScorer
    {
        private readonly PredictionEnginePool<UserInput, UserOutput> _predictionEnginePool;
        public AggressionScorer(PredictionEnginePool<UserInput, UserOutput> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool ?? throw new ArgumentNullException(nameof(predictionEnginePool));
        }

        public UserOutput Predict(string input)
        {
            if (_predictionEnginePool == null) {
                throw new InvalidOperationException("PredictionEnginePool is not initialized.");
            }

            return _predictionEnginePool.Predict(new UserInput() { Comment = input });
        }
    }
}
