using Microsoft.ML.Data;

namespace AggressionScorerModel
{
    public class UserInput
    {
        [LoadColumn(1)]
        public string? Comment { get; set; }

        [LoadColumn(0), ColumnName("Label")]
        public bool Aggressive { get; set; }
    }
}
