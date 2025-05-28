namespace SkillBridgeDentistry1.Dtos
{
    public class AIResponseDto

    {
        public string prediction { get; set; }
        public double confidence { get; set; }
        public Dictionary<string, double> probabilities { get; set; }

        public string treatment { get; set; }
    }

}