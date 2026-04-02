namespace ai_apps_01.models;

record SentimentAnalysis(
    string ResponseText,
    Sentiment ReviewSentiment,
    double ConfidenceScore,
    string[] KeyPhrases
);