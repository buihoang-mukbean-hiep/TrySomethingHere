namespace InstantWording
{
    public class Word
    {
        private readonly int[] frequencyPool = [0, 1, 3, 7, 14, 30, 60];

        public required string Id { get; set; }
        public string? Kanji { get; set; }
        public string? Hiragana { get; set; }
        public string? Kanji_Vietnamese { get; set; }
        public string? Definition { get; set; }
        public int RememberLevel { get; set; }
        public DateTime LastReviewDate { get; set; }
        public int ReviewTimes { get; set; }
        public int NextReviewLeft
        {
            get
            =>  frequencyPool[this.ReviewTimes] - (DateTime.Now - LastReviewDate).Days;
            set { }
        }

    }
}
