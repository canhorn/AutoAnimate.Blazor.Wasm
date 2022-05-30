namespace AutoAnimate.Blazor.Wasm.Pages;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

public partial class List
{
    [Inject]
    public IJSRuntime JSRuntime { get; set; } = null!;

    private ElementReference listRef;
    private IJSObjectReference? _module;

    private List<string> items = new List<string>
    {
        "\ud83c\udf4e Apple",
        "\ud83c\udf4c\u0020\u0020 Banana",
        "\ud83c\udf52Cherry"
    };
    private readonly List<string> fruitBasket = new List<string>
    {
        "\ud83c\udf53 Strawberry",
        "\ud83e\udd65 Coconut",
        "\ud83e\udd5d Kiwi",
        "\ud83c\udf47 Grape",
        "\ud83c\udf49 Watermelon",
        "\ud83c\udf4d Pineapple",
        "\ud83c\udf50 Pear",
        "\ud83c\udf51 Peach",
        "\ud83e\uded0 Blueberry",
        "\ud83c\udf4a Orange",
        "\ud83e\udd6d Mango",
    };

    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "https://cdn.jsdelivr.net/npm/@formkit/auto-animate@1.0.0-beta.1/index.min.js"
            );
            await _module.InvokeVoidAsync("default", listRef);
        }
    }

    private void AddFruit()
    {
        var item = fruitBasket.FirstOrDefault();
        if (item is not null)
        {
            items.Add(item);
            fruitBasket.Remove(item);
        }
    }

    private void RemoveItem(string item)
    {
        items.Remove(item);
        fruitBasket.Add(item);
    }

    private void Shuffle()
    {
        items = items.OrderBy(a => Guid.NewGuid().ToString()).ToList();
    }
}
