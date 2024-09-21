using System.Globalization;

namespace AggressionScorer
{
    public class DataPreparer
    {
        public static void CreatePreparedDataFile(string outputFile, bool onlySaveSmallSubset = false)
        {;
            string filePath = "C:\\Users\\navva\\OneDrive\\Desktop\\ML.NET_Basic_Projects\\AggressionScorer\\AggressionScorer\\Data\\aggression_annotations.tsv";
            var annotations = File.ReadAllLines(filePath).Skip(1);

            var aggressiveScoreMap = new Dictionary<int, List<int>>();

            //Collect all aggression ratings for each comment (revId)
            foreach (var annotation in annotations)
            {
                var parts = annotation.Split('\t');

                var revId = int.Parse(parts[0]);
                var aggressiveScore = (int)double.Parse(parts[3], CultureInfo.InvariantCulture);

                if (aggressiveScoreMap.ContainsKey(revId))
                {

                    aggressiveScoreMap[revId].Add(aggressiveScore);

                }
                else
                {
                    aggressiveScoreMap[revId] = new List<int>() { aggressiveScore };
                }
            }

            // Pair all comments with aggression score
            filePath = "C:\\Users\\navva\\OneDrive\\Desktop\\ML.NET_Basic_Projects\\AggressionScorer\\AggressionScorer\\Data\\aggression_annotations.tsv";
            var allComments = File.ReadAllLines(filePath).Skip(1);

            var formattedOutput = allComments.Select(c =>
            {
                var inputLineParts = c.Split('\t');

                var commentId = int.Parse(inputLineParts[0]);

                var aggressionScores = aggressiveScoreMap[commentId];

                var aggression = aggressionScores.Average() < -0.9 ? 1 : 0;

                var comment = inputLineParts[1].Replace("NEWLINE_TOKEN", "");
                return $"{aggression}\t{comment}";
            });

            // Take the small or the big subset of the data
            var finalOutput = onlySaveSmallSubset ?
                formattedOutput.Take(3000).ToList() :
                formattedOutput.Skip(3000).ToList();

            //var finalOutput = formattedOutput.ToList();

            finalOutput.Insert(0, "IsAggressive\tComment");

            //Write the new file to use as ML.Net input
            File.WriteAllLines(outputFile, finalOutput);
        }
    }
}