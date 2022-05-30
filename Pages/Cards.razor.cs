namespace AutoAnimate.Blazor.Wasm.Pages;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

public partial class Cards
{
    [Inject]
    public IJSRuntime JSRuntime { get; set; } = null!;

    public bool _showForm = false;
    public ElementReference _cardsContainerRef;
    public ElementReference _cardsRef;
    private IJSObjectReference? _module;
    private IEnumerable<Card> _cards = new List<Card>
    {
        new Card(
            Guid.NewGuid().ToString(),
            "Employee Lunch",
            "Join us for an all hands meeting",
            "March 31"
        ),
        new Card(
            Guid.NewGuid().ToString(),
            "Engineering scrum",
            "Engineering team is playing rugby",
            "March 31"
        ),
    };

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "https://cdn.jsdelivr.net/npm/@formkit/auto-animate@1.0.0-beta.1/index.min.js"
            );
            await _module.InvokeVoidAsync("default", _cardsContainerRef);
            await _module.InvokeVoidAsync("default", _cardsRef);
        }
    }

    private void HandleSubmit()
    {
        if (
            string.IsNullOrEmpty(Title)
            || string.IsNullOrEmpty(Date)
            || string.IsNullOrEmpty(Description)
        )
        {
            return;
        }
        var cards = _cards.ToList();
        cards.Add(new Card(Guid.NewGuid().ToString(), Title, Date, Description));
        _cards = cards;
        Shuffle();

        _showForm = false;
    }

    private async Task ShowForm()
    {
        _showForm = true;
        await Task.Delay(1);
        await _titleRef.Element.Value.FocusAsync();
    }

    private void Shuffle()
    {
        _cards = _cards.OrderBy(a => Guid.NewGuid().ToString()).ToList();
    }
}

public record Card(string Id, string Title, string Description, string Date);
