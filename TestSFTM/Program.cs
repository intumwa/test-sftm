using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tree_matching_csharp;
using tree_matching_csharp.Benchmark;

namespace TestSFTM
{
    class Program
    {
        public static SftmTreeMatcher.Parameters _parameters = new SftmTreeMatcher.Parameters
        {
            LimitNeighbors = 100,
            MetropolisParameters = new Metropolis.Parameters
            {
                Gamma = 0.9f,
                Lambda = 0.7f,
                NbIterations = 10,
            },
            NoMatchCost = 1.2,
            MaxPenalizationChildren = 0.4,
            MaxPenalizationParentsChildren = 0.2,
            PropagationParameters = new SimilarityPropagation.Parameters()
            {
                Envelop = new[] { 0.8, 0.1, 0.01 },
                // Envelop    = new[] {0.0},
                Parent = 0.25,
                Sibling = 0.0,
                SiblingInv = 0.0,
                ParentInv = 0.7,
                Children = 0.1
            },
            MaxTokenAppearance = n => (int)Math.Sqrt(n)
        };
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Testing the SFTM!");
            Console.WriteLine("-----------------");
            Console.WriteLine("");
            Console.WriteLine("");

            var webDOM1 = await DOM.WebpageToDocument(File.ReadAllText(@"websites/simple.html"));
            var webDOM2 = await DOM.WebpageToDocument(File.ReadAllText(@"websites/simple_changed.html"));

            var source = DOM.DomToTree(webDOM1);
            var target = DOM.DomToTree(webDOM2);

            var treeMatcher = new SftmTreeMatcher(_parameters);
            var resultMatching = await treeMatcher.MatchTrees(source, target);

            var edges = resultMatching.Edges;

            foreach (var edge in edges)
            {
                var s = "";
                if (edge.Source != null)
                    s = edge.Source.Value[0];
                else
                    s = "-";

                var t = "";
                if (edge.Target != null)
                    t = edge.Target.Value[0];
                else
                    t = "-";

                Console.WriteLine("{0} | {1}", s, t);


            }

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("----------------------");
            Console.WriteLine("Done testing the SFTM.");
            Console.ReadLine();
        }
    }
}
