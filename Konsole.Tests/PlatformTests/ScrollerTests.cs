using FluentAssertions;
using Goblinfactory.Konsole.Platform;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Goblinfactory.Konsole.Platform.Windows.Kernel32Draw;

namespace Konsole.Tests.PlatformTests
{
    [TestFixture]
    public class ScrollerTests
    {
        [Test]
        public void WhenScrollingWholeScreen_ScrollDownBy1_Should_ScrollTheWholeScreenDown1Line()
        {
            var buffer = BufferBuilder.BuildBuffer(7, 5, '.', Colors.WhiteOnBlack, 
                "the1234", 
                "gray[=]",
                "cats567", 
                "howled8",
                "9012345"
            );
            var scroller = new Scroller(buffer, 7, 4, '.', Colors.WhiteOnBlack);
            scroller.ScrollDown(1, 0, 0, 7, 4);
            var actual = buffer.ToSingleString();
            actual.Should().Be(
                "gray[=]" +
                "cats567" +
                "howled8" +
                "9012345" +
                "......."
            );
        }

        [Test]
        public void WhenScrollingWholeScreen_ScrollDownBy2_Should_ScrollTheWholeScreenDown2Lines()
        {
            var buffer = BufferBuilder.BuildBuffer(7, 5, '.', Colors.WhiteOnBlack,
                "the....",
                "gray...",
                "cats...",
                "howled.",
                "crazily"
            );
            var scroller = new Scroller(buffer, 7, 4, '.', Colors.WhiteOnBlack);
            scroller.ScrollDown(2, 0, 0, 7, 4);
            var actual = buffer.ToSingleString();
            actual.Should().Be(
                "cats..." +
                "howled." +
                "crazily" +
                "......." +
                "......."
            );

        }


        //[Test]
        //public void WhenScrollingRegionOfAScreen_ScrollDownShould_ScrollOnlyTheRegion()
        //{

        //}

        [Test]
        public void BufferBuilderTests()
        {
            var buffer = BufferBuilder.BuildBuffer(7, 4, '.', Colors.WhiteOnBlack, "the", "cats", "howled");
            var actual = buffer.ToSingleString();
            actual.Should().Be(
                "the...." +
                "cats..." +
                "howled." +
                ".......");
        }

        internal static class BufferBuilder
        {
            public static CharAndColor[] BuildBuffer(int width,int height, char defaultChar, Colors colors, params string[] lines)
            {
                int size = height * width;
                var buffer = new CharAndColor[size];

                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        int offset = y * width + x;
                        buffer[offset] = colors.SetChar(defaultChar);
                    }
                
                for(int i = 0; i< lines.Length; i++)
                    for(int x = 0; x<lines[i].Length; x++)
                    {
                        int offset = i * width + x;
                        buffer[offset] = colors.SetChar(lines[i][x]);
                    }
                return buffer;
            }
        }
    }
    internal static class BufferExtensions
    {
        public static string ToSingleString(this CharAndColor[] src)
        {
            return new String(src.Select(b => b.Char.UnicodeChar).ToArray());
        }
    }
}
