namespace ai_apps_01.models;

record ActionItem(
    string Task,
    string? Assignee,
    string? DueDate,
    string Priority
);