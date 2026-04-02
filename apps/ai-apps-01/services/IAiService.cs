namespace ai_apps_01.services;

public interface IAiService
{
    public Task RunFirstSampel();

    // without using streaming --> not good, in long answer user just look for a black screen.
    public Task SimpleChatApp();

    public Task SimpleChatAppWithStreamiing();

    public Task SimpleStracturedOutput();

    public Task SimpleStracturedOutputWithMultipleItems();

    public Task ComplexStracturedOutput();

    public Task ActionItemSample();
}