using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests.PathTests
{
    [TestFixture]
    public class LongFileNameTests
    {
        [Test]
        public void TestLongFileName()
        {
            var filename = @"S:\Export\EOLWave2_Pt2\skysis.sharesrvr.com\Associates/Marketing and Business Development/Marketing Resources/Case Studies/Case Studies_PDF Format\Skysis_Case_Study_Consumer_Global Launch of Product Portfolios in Europe, Asia, Latin America and Middle East.pdf";
            Console.WriteLine(filename.Length);

            var shortened = @"aegerion.sharesrvr.com\Brazil MAA/Lomitapide CTD Documents/Module 3 - Quality Body of Data/32-body-data-FOR TRANSLATION/32-body-data/32r-reg-info/Drug Substance Method Validation Protocols                     NO\Aeg-P_-a6c739";
            Console.WriteLine(shortened.Length);
        }
    }
}
