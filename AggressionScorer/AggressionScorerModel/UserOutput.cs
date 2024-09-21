using Microsoft.ML.Data;

namespace AggressionScorerModel
{
    public class UserOutput
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set;}
        public float Probability { get; set;}
    }
}
