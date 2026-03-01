
namespace Application.Test;

public class ExampleTests
{

    [Test]
    public async Task ExempleTest()
    {
        var sum = Add(3,2);

        await Assert.That(sum).IsEqualTo(5);
    }

    private static int Add(int x, int y) => x+y;
}
