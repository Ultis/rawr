using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr
{
    // data from http://code.google.com/p/simulationcraft/source/browse/branches/cataclysm/engine/sc_item_data.inc
    public static class RandomSuffix
    {
        private static readonly int[,,] RandomPropData = {
{ /*277*/ {  348,  258,  194,  149,  109 }, {  302,  225,  168,  130,   95 }, {  302,  225,  168,  130,   95 } },
{ /*278*/ {  351,  261,  195,  150,  110 }, {  305,  227,  170,  131,   96 }, {  305,  227,  170,  131,   96 } },
{ /*279*/ {  354,  263,  197,  152,  111 }, {  308,  229,  172,  132,   97 }, {  308,  229,  172,  132,   97 } },
{ /*280*/ {  357,  266,  199,  153,  112 }, {  311,  231,  173,  133,   98 }, {  311,  231,  173,  133,   98 } },
{ /*281*/ {  361,  268,  201,  155,  113 }, {  314,  233,  175,  134,   99 }, {  314,  233,  175,  134,   99 } },
{ /*282*/ {  364,  271,  203,  156,  114 }, {  317,  235,  176,  136,  100 }, {  317,  235,  176,  136,  100 } },
{ /*283*/ {  368,  273,  205,  158,  116 }, {  320,  237,  178,  137,  100 }, {  320,  237,  178,  137,  100 } },
{ /*284*/ {  371,  276,  207,  159,  117 }, {  323,  240,  180,  138,  101 }, {  323,  240,  180,  138,  101 } },
{ /*285*/ {  326,  242,  181,  140,  102 }, {  326,  242,  181,  140,  102 }, {  326,  242,  181,  140,  102 } },
{ /*286*/ {  329,  244,  183,  141,  103 }, {  329,  244,  183,  141,  103 }, {  329,  244,  183,  141,  103 } },
{ /*287*/ {  332,  246,  185,  142,  104 }, {  332,  246,  185,  142,  104 }, {  332,  246,  185,  142,  104 } },
{ /*288*/ {  335,  249,  187,  144,  105 }, {  335,  249,  187,  144,  105 }, {  335,  249,  187,  144,  105 } },
{ /*289*/ {  338,  251,  188,  145,  106 }, {  338,  251,  188,  145,  106 }, {  338,  251,  188,  145,  106 } },
{ /*290*/ {  341,  253,  190,  146,  107 }, {  341,  253,  190,  146,  107 }, {  341,  253,  190,  146,  107 } },
{ /*291*/ {  344,  256,  192,  148,  108 }, {  344,  256,  192,  148,  108 }, {  344,  256,  192,  148,  108 } },
{ /*292*/ {  348,  258,  194,  149,  109 }, {  348,  258,  194,  149,  109 }, {  348,  258,  194,  149,  109 } },
{ /*293*/ {  351,  261,  195,  150,  110 }, {  351,  261,  195,  150,  110 }, {  351,  261,  195,  150,  110 } },
{ /*294*/ {  354,  263,  197,  152,  111 }, {  354,  263,  197,  152,  111 }, {  354,  263,  197,  152,  111 } },
{ /*295*/ {  357,  266,  199,  153,  112 }, {  357,  266,  199,  153,  112 }, {  357,  266,  199,  153,  112 } },
{ /*296*/ {  361,  268,  201,  155,  113 }, {  361,  268,  201,  155,  113 }, {  361,  268,  201,  155,  113 } },
{ /*297*/ {  364,  271,  203,  156,  114 }, {  364,  271,  203,  156,  114 }, {  364,  271,  203,  156,  114 } },
{ /*298*/ {  368,  273,  205,  158,  116 }, {  368,  273,  205,  158,  116 }, {  368,  273,  205,  158,  116 } },
{ /*299*/ {  371,  276,  207,  159,  117 }, {  371,  276,  207,  159,  117 }, {  371,  276,  207,  159,  117 } },
{ /*300*/ {  375,  278,  209,  161,  118 }, {  375,  278,  209,  161,  118 }, {  375,  278,  209,  161,  118 } },
{ /*301*/ {  378,  281,  211,  162,  119 }, {  378,  281,  211,  162,  119 }, {  378,  281,  211,  162,  119 } },
{ /*302*/ {  382,  283,  213,  164,  120 }, {  382,  283,  213,  164,  120 }, {  382,  283,  213,  164,  120 } },
{ /*303*/ {  385,  286,  215,  165,  121 }, {  385,  286,  215,  165,  121 }, {  385,  286,  215,  165,  121 } },
{ /*304*/ {  389,  289,  217,  167,  122 }, {  389,  289,  217,  167,  122 }, {  389,  289,  217,  167,  122 } },
{ /*305*/ {  392,  291,  219,  168,  123 }, {  392,  291,  219,  168,  123 }, {  392,  291,  219,  168,  123 } },
{ /*306*/ {  396,  294,  221,  170,  124 }, {  396,  294,  221,  170,  124 }, {  396,  294,  221,  170,  124 } },
{ /*307*/ {  400,  297,  223,  171,  126 }, {  400,  297,  223,  171,  126 }, {  400,  297,  223,  171,  126 } },
{ /*308*/ {  404,  300,  225,  173,  127 }, {  404,  300,  225,  173,  127 }, {  404,  300,  225,  173,  127 } },
{ /*309*/ {  407,  303,  227,  175,  128 }, {  407,  303,  227,  175,  128 }, {  407,  303,  227,  175,  128 } },
{ /*310*/ {  411,  305,  229,  176,  129 }, {  411,  305,  229,  176,  129 }, {  411,  305,  229,  176,  129 } },
{ /*311*/ {  415,  308,  231,  178,  130 }, {  415,  308,  231,  178,  130 }, {  415,  308,  231,  178,  130 } },
{ /*312*/ {  419,  311,  233,  179,  132 }, {  419,  311,  233,  179,  132 }, {  419,  311,  233,  179,  132 } },
{ /*313*/ {  423,  314,  236,  181,  133 }, {  423,  314,  236,  181,  133 }, {  423,  314,  236,  181,  133 } },
{ /*314*/ {  427,  317,  238,  183,  134 }, {  427,  317,  238,  183,  134 }, {  427,  317,  238,  183,  134 } },
{ /*315*/ {  431,  320,  240,  185,  135 }, {  431,  320,  240,  185,  135 }, {  431,  320,  240,  185,  135 } },
{ /*316*/ {  435,  323,  242,  186,  137 }, {  435,  323,  242,  186,  137 }, {  435,  323,  242,  186,  137 } },
{ /*317*/ {  439,  326,  244,  188,  138 }, {  439,  326,  244,  188,  138 }, {  439,  326,  244,  188,  138 } },
{ /*318*/ {  443,  329,  247,  190,  139 }, {  443,  329,  247,  190,  139 }, {  443,  329,  247,  190,  139 } },
{ /*319*/ {  447,  332,  249,  192,  141 }, {  447,  332,  249,  192,  141 }, {  447,  332,  249,  192,  141 } },
{ /*320*/ {  451,  335,  251,  193,  142 }, {  451,  335,  251,  193,  142 }, {  451,  335,  251,  193,  142 } },
{ /*321*/ {  455,  338,  254,  195,  143 }, {  455,  338,  254,  195,  143 }, {  455,  338,  254,  195,  143 } },
{ /*322*/ {  460,  342,  256,  197,  144 }, {  460,  342,  256,  197,  144 }, {  460,  342,  256,  197,  144 } },
{ /*323*/ {  464,  345,  259,  199,  146 }, {  464,  345,  259,  199,  146 }, {  464,  345,  259,  199,  146 } },
{ /*324*/ {  468,  348,  261,  201,  147 }, {  468,  348,  261,  201,  147 }, {  468,  348,  261,  201,  147 } },
{ /*325*/ {  473,  351,  263,  203,  149 }, {  473,  351,  263,  203,  149 }, {  473,  351,  263,  203,  149 } },
{ /*326*/ {  477,  354,  266,  205,  150 }, {  477,  354,  266,  205,  150 }, {  477,  354,  266,  205,  150 } },
{ /*327*/ {  482,  358,  268,  206,  151 }, {  482,  358,  268,  206,  151 }, {  482,  358,  268,  206,  151 } },
{ /*328*/ {  486,  361,  271,  208,  153 }, {  486,  361,  271,  208,  153 }, {  486,  361,  271,  208,  153 } },
{ /*329*/ {  491,  365,  273,  210,  154 }, {  491,  365,  273,  210,  154 }, {  491,  365,  273,  210,  154 } },
{ /*330*/ {  495,  368,  276,  212,  156 }, {  495,  368,  276,  212,  156 }, {  495,  368,  276,  212,  156 } },
{ /*331*/ {  500,  371,  279,  214,  157 }, {  500,  371,  279,  214,  157 }, {  500,  371,  279,  214,  157 } },
{ /*332*/ {  505,  375,  281,  216,  159 }, {  505,  375,  281,  216,  159 }, {  505,  375,  281,  216,  159 } },
{ /*333*/ {  509,  378,  284,  218,  160 }, {  509,  378,  284,  218,  160 }, {  509,  378,  284,  218,  160 } },
{ /*334*/ {  514,  382,  286,  220,  162 }, {  514,  382,  286,  220,  162 }, {  514,  382,  286,  220,  162 } },
{ /*335*/ {  519,  385,  289,  222,  163 }, {  519,  385,  289,  222,  163 }, {  519,  385,  289,  222,  163 } },
{ /*336*/ {  524,  389,  292,  224,  165 }, {  524,  389,  292,  224,  165 }, {  524,  389,  292,  224,  165 } },
{ /*337*/ {  529,  393,  295,  227,  166 }, {  529,  393,  295,  227,  166 }, {  529,  393,  295,  227,  166 } },
{ /*338*/ {  534,  396,  297,  229,  168 }, {  534,  396,  297,  229,  168 }, {  534,  396,  297,  229,  168 } },
{ /*339*/ {  539,  400,  300,  231,  169 }, {  539,  400,  300,  231,  169 }, {  539,  400,  300,  231,  169 } },
{ /*340*/ {  544,  404,  303,  233,  171 }, {  544,  404,  303,  233,  171 }, {  544,  404,  303,  233,  171 } },
{ /*341*/ {  549,  408,  306,  235,  172 }, {  549,  408,  306,  235,  172 }, {  549,  408,  306,  235,  172 } },
{ /*342*/ {  554,  411,  309,  237,  174 }, {  554,  411,  309,  237,  174 }, {  554,  411,  309,  237,  174 } },
{ /*343*/ {  559,  415,  311,  240,  176 }, {  559,  415,  311,  240,  176 }, {  559,  415,  311,  240,  176 } },
{ /*344*/ {  564,  419,  314,  242,  177 }, {  564,  419,  314,  242,  177 }, {  564,  419,  314,  242,  177 } },
{ /*345*/ {  570,  423,  317,  244,  179 }, {  570,  423,  317,  244,  179 }, {  570,  423,  317,  244,  179 } },
{ /*346*/ {  575,  427,  320,  246,  181 }, {  575,  427,  320,  246,  181 }, {  575,  427,  320,  246,  181 } },
{ /*347*/ {  580,  431,  323,  249,  182 }, {  580,  431,  323,  249,  182 }, {  580,  431,  323,  249,  182 } },
{ /*348*/ {  586,  435,  326,  251,  184 }, {  586,  435,  326,  251,  184 }, {  586,  435,  326,  251,  184 } },
{ /*349*/ {  591,  439,  329,  253,  186 }, {  591,  439,  329,  253,  186 }, {  591,  439,  329,  253,  186 } },
{ /*350*/ {  597,  443,  332,  256,  188 }, {  597,  443,  332,  256,  188 }, {  597,  443,  332,  256,  188 } },
{ /*351*/ {  602,  447,  336,  258,  189 }, {  602,  447,  336,  258,  189 }, {  602,  447,  336,  258,  189 } },
{ /*352*/ {  608,  452,  339,  261,  191 }, {  608,  452,  339,  261,  191 }, {  608,  452,  339,  261,  191 } },
{ /*353*/ {  614,  456,  342,  263,  193 }, {  614,  456,  342,  263,  193 }, {  614,  456,  342,  263,  193 } },
{ /*354*/ {  619,  460,  345,  265,  195 }, {  619,  460,  345,  265,  195 }, {  619,  460,  345,  265,  195 } },
{ /*355*/ {  625,  464,  348,  268,  196 }, {  625,  464,  348,  268,  196 }, {  625,  464,  348,  268,  196 } },
{ /*356*/ {  631,  469,  352,  270,  198 }, {  631,  469,  352,  270,  198 }, {  631,  469,  352,  270,  198 } },
{ /*357*/ {  637,  473,  355,  273,  200 }, {  637,  473,  355,  273,  200 }, {  637,  473,  355,  273,  200 } },
{ /*358*/ {  643,  478,  358,  276,  202 }, {  643,  478,  358,  276,  202 }, {  643,  478,  358,  276,  202 } },
{ /*359*/ {  649,  482,  362,  278,  204 }, {  649,  482,  362,  278,  204 }, {  649,  482,  362,  278,  204 } },
{ /*360*/ {  655,  487,  365,  281,  206 }, {  655,  487,  365,  281,  206 }, {  655,  487,  365,  281,  206 } },
{ /*361*/ {  661,  491,  368,  283,  208 }, {  661,  491,  368,  283,  208 }, {  661,  491,  368,  283,  208 } },
{ /*362*/ {  667,  496,  372,  286,  210 }, {  667,  496,  372,  286,  210 }, {  667,  496,  372,  286,  210 } },
{ /*363*/ {  674,  500,  375,  289,  212 }, {  674,  500,  375,  289,  212 }, {  674,  500,  375,  289,  212 } },
{ /*364*/ {  680,  505,  379,  291,  214 }, {  680,  505,  379,  291,  214 }, {  680,  505,  379,  291,  214 } },
{ /*365*/ {  686,  510,  382,  294,  216 }, {  686,  510,  382,  294,  216 }, {  686,  510,  382,  294,  216 } },
{ /*366*/ {  693,  515,  386,  297,  218 }, {  693,  515,  386,  297,  218 }, {  693,  515,  386,  297,  218 } },
{ /*367*/ {  699,  519,  390,  300,  220 }, {  699,  519,  390,  300,  220 }, {  699,  519,  390,  300,  220 } },
{ /*368*/ {  706,  524,  393,  302,  222 }, {  706,  524,  393,  302,  222 }, {  706,  524,  393,  302,  222 } },
{ /*369*/ {  712,  529,  397,  305,  224 }, {  712,  529,  397,  305,  224 }, {  712,  529,  397,  305,  224 } },
{ /*370*/ {  719,  534,  401,  308,  226 }, {  719,  534,  401,  308,  226 }, {  719,  534,  401,  308,  226 } },
{ /*371*/ {  726,  539,  404,  311,  228 }, {  726,  539,  404,  311,  228 }, {  726,  539,  404,  311,  228 } },
{ /*372*/ {  733,  544,  408,  314,  230 }, {  733,  544,  408,  314,  230 }, {  733,  544,  408,  314,  230 } },
{ /*373*/ {  739,  549,  412,  317,  232 }, {  739,  549,  412,  317,  232 }, {  739,  549,  412,  317,  232 } },
{ /*374*/ {  746,  554,  416,  320,  235 }, {  746,  554,  416,  320,  235 }, {  746,  554,  416,  320,  235 } },
{ /*375*/ {  753,  560,  420,  323,  237 }, {  753,  560,  420,  323,  237 }, {  753,  560,  420,  323,  237 } },
{ /*376*/ {  760,  565,  424,  326,  239 }, {  760,  565,  424,  326,  239 }, {  760,  565,  424,  326,  239 } },
{ /*377*/ {  767,  570,  428,  329,  241 }, {  767,  570,  428,  329,  241 }, {  767,  570,  428,  329,  241 } },
{ /*378*/ {  775,  575,  432,  332,  243 }, {  775,  575,  432,  332,  243 }, {  775,  575,  432,  332,  243 } },
{ /*379*/ {  782,  581,  436,  335,  246 }, {  782,  581,  436,  335,  246 }, {  782,  581,  436,  335,  246 } },
{ /*380*/ {  789,  586,  440,  338,  248 }, {  789,  586,  440,  338,  248 }, {  789,  586,  440,  338,  248 } },
{ /*381*/ {  797,  592,  444,  341,  250 }, {  797,  592,  444,  341,  250 }, {  797,  592,  444,  341,  250 } },
{ /*382*/ {  804,  597,  448,  345,  253 }, {  804,  597,  448,  345,  253 }, {  804,  597,  448,  345,  253 } },
{ /*383*/ {  812,  603,  452,  348,  255 }, {  812,  603,  452,  348,  255 }, {  812,  603,  452,  348,  255 } },
{ /*384*/ {  819,  609,  456,  351,  257 }, {  819,  609,  456,  351,  257 }, {  819,  609,  456,  351,  257 } },
{ /*385*/ {  827,  614,  461,  354,  260 }, {  827,  614,  461,  354,  260 }, {  827,  614,  461,  354,  260 } },
{ /*386*/ {  835,  620,  465,  358,  262 }, {  835,  620,  465,  358,  262 }, {  835,  620,  465,  358,  262 } },
{ /*387*/ {  842,  626,  469,  361,  265 }, {  842,  626,  469,  361,  265 }, {  842,  626,  469,  361,  265 } },
{ /*388*/ {  850,  632,  474,  364,  267 }, {  850,  632,  474,  364,  267 }, {  850,  632,  474,  364,  267 } },
{ /*389*/ {  858,  638,  478,  368,  270 }, {  858,  638,  478,  368,  270 }, {  858,  638,  478,  368,  270 } },
{ /*390*/ {  866,  644,  483,  371,  272 }, {  866,  644,  483,  371,  272 }, {  866,  644,  483,  371,  272 } },
{ /*391*/ {  874,  650,  487,  375,  275 }, {  874,  650,  487,  375,  275 }, {  874,  650,  487,  375,  275 } },
{ /*392*/ {  883,  656,  492,  378,  277 }, {  883,  656,  492,  378,  277 }, {  883,  656,  492,  378,  277 } },
{ /*393*/ {  891,  662,  496,  382,  280 }, {  891,  662,  496,  382,  280 }, {  891,  662,  496,  382,  280 } },
{ /*394*/ {  899,  668,  501,  385,  283 }, {  899,  668,  501,  385,  283 }, {  899,  668,  501,  385,  283 } },
{ /*395*/ {  908,  674,  506,  389,  285 }, {  908,  674,  506,  389,  285 }, {  908,  674,  506,  389,  285 } },
{ /*396*/ {  916,  681,  510,  393,  288 }, {  916,  681,  510,  393,  288 }, {  916,  681,  510,  393,  288 } },
{ /*397*/ {  925,  687,  515,  396,  291 }, {  925,  687,  515,  396,  291 }, {  925,  687,  515,  396,  291 } },
{ /*398*/ {  933,  693,  520,  400,  293 }, {  933,  693,  520,  400,  293 }, {  933,  693,  520,  400,  293 } },
{ /*399*/ {  942,  700,  525,  404,  296 }, {  942,  700,  525,  404,  296 }, {  942,  700,  525,  404,  296 } },
{ /*400*/ {  951,  706,  530,  408,  299 }, {  951,  706,  530,  408,  299 }, {  951,  706,  530,  408,  299 } }, 
        };

        private class RandomSuffixDataType
        {
            public int Id;
            public string Suffix;
            public int[] Stat;
            public int[] Multiplier;
        }

        private static RandomSuffixDataType[] FlattenArray(RandomSuffixDataType[] data)
        {
            int maxId = data[data.Length - 1].Id;
            RandomSuffixDataType[] ret = new RandomSuffixDataType[maxId + 1];
            foreach (var d in data)
            {
                ret[d.Id] = d;
            }
            return ret;
        }

        private static readonly RandomSuffixDataType[] RandomSuffixData = FlattenArray(new RandomSuffixDataType[]{
new RandomSuffixDataType() { Id = 5, Suffix = "of the Monkey",          Stat = new int[] {  2802,  2803,     0,     0,     0 }, Multiplier = new int[] {  6666, 10000,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 6, Suffix = "of the Eagle",           Stat = new int[] {  2804,  2803,     0,     0,     0 }, Multiplier = new int[] {  6666, 10000,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 7, Suffix = "of the Bear",            Stat = new int[] {  2803,  2805,     0,     0,     0 }, Multiplier = new int[] { 10000,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 8, Suffix = "of the Whale",           Stat = new int[] {  2806,  2803,     0,     0,     0 }, Multiplier = new int[] {  6666, 10000,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 9, Suffix = "of the Owl",             Stat = new int[] {  2804,  2806,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 10, Suffix = "of the Gorilla",        Stat = new int[] {  2804,  2805,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 11, Suffix = "of the Falcon",         Stat = new int[] {  2802,  2804,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 12, Suffix = "of the Boar",           Stat = new int[] {  2806,  2805,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 13, Suffix = "of the Wolf",           Stat = new int[] {  2802,  2806,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 14, Suffix = "of the Tiger",          Stat = new int[] {  2802,  3727,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 15, Suffix = "of Spirit",             Stat = new int[] {  2806,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 16, Suffix = "of Stamina",            Stat = new int[] {  2803,     0,     0,     0,     0 }, Multiplier = new int[] { 15000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 17, Suffix = "of Strength",           Stat = new int[] {  2805,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 18, Suffix = "of Agility",            Stat = new int[] {  2802,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 19, Suffix = "of Intellect",          Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 20, Suffix = "of Power",              Stat = new int[] {  2825,     0,     0,     0,     0 }, Multiplier = new int[] { 20000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 21, Suffix = "of Intellect",          Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 22, Suffix = "of Intellect",          Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 23, Suffix = "of Intellect",          Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 24, Suffix = "of Intellect",          Stat = new int[] {  2824,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 25, Suffix = "of Intellect",          Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 26, Suffix = "of Intellect",          Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 27, Suffix = "of Nimbleness",         Stat = new int[] {  2815,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 28, Suffix = "of Stamina",            Stat = new int[] {  2803,     0,     0,     0,     0 }, Multiplier = new int[] { 15000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 29, Suffix = "of Eluding",            Stat = new int[] {  2815,  2802,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 30, Suffix = "of Spirit",             Stat = new int[] {  2806,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 36, Suffix = "of the Sorcerer",       Stat = new int[] {  2803,  2804,  3726,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 37, Suffix = "of the Seer",           Stat = new int[] {  2803,  2804,  2822,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 38, Suffix = "of the Prophet",        Stat = new int[] {  2804,  2806,  3726,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 39, Suffix = "of the Invoker",        Stat = new int[] {  2804,  2822,     0,     0,     0 }, Multiplier = new int[] {  7889,  5259,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 40, Suffix = "of the Bandit",         Stat = new int[] {  2802,  2803,  2822,     0,     0 }, Multiplier = new int[] {  5259,  7889,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 41, Suffix = "of the Beast",          Stat = new int[] {  3727,  2822,  2803,     0,     0 }, Multiplier = new int[] {  5259,  5259,  7889,     0,     0 } },
new RandomSuffixDataType() { Id = 42, Suffix = "of the Elder",          Stat = new int[] {  2803,  2806,  2804,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 43, Suffix = "of the Soldier",        Stat = new int[] {  2805,  2803,  2823,     0,     0 }, Multiplier = new int[] {  5259,  7889,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 44, Suffix = "of the Elder",          Stat = new int[] {  2803,  2804,  2806,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 45, Suffix = "of the Champion",       Stat = new int[] {  2805,  2803,  2815,     0,     0 }, Multiplier = new int[] {  5259,  7889,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 47, Suffix = "of Blocking",           Stat = new int[] {  2826,  2805,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 49, Suffix = "of the Grove",          Stat = new int[] {  2805,  2802,  2803,     0,     0 }, Multiplier = new int[] {  7266,  4106,  4790,     0,     0 } },
new RandomSuffixDataType() { Id = 50, Suffix = "of the Hunt",           Stat = new int[] {  2825,  2802,  2804,     0,     0 }, Multiplier = new int[] { 14532,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 51, Suffix = "of the Mind",           Stat = new int[] {  2824,  2822,  2804,     0,     0 }, Multiplier = new int[] {  8501,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 52, Suffix = "of the Crusade",        Stat = new int[] {  2824,  2804,  2813,     0,     0 }, Multiplier = new int[] {  8501,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 53, Suffix = "of the Vision",         Stat = new int[] {  2824,  2804,  2803,     0,     0 }, Multiplier = new int[] {  8501,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 54, Suffix = "of the Ancestor",       Stat = new int[] {  2805,  2823,  2803,     0,     0 }, Multiplier = new int[] {  7266,  4106,  4790,     0,     0 } },
new RandomSuffixDataType() { Id = 56, Suffix = "of the Battle",         Stat = new int[] {  2805,  2803,  2823,     0,     0 }, Multiplier = new int[] {  7266,  6159,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 57, Suffix = "of the Shadow",         Stat = new int[] {  2825,  2802,  2803,     0,     0 }, Multiplier = new int[] { 14532,  4106,  4790,     0,     0 } },
new RandomSuffixDataType() { Id = 58, Suffix = "of the Sun",            Stat = new int[] {  2823,  2803,  2804,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 59, Suffix = "of the Moon",           Stat = new int[] {  2804,  2803,  2806,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 60, Suffix = "of the Wild",           Stat = new int[] {  2825,  2803,  2802,     0,     0 }, Multiplier = new int[] { 10518,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 61, Suffix = "of Intellect",          Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] {  2273,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 62, Suffix = "of Strength",           Stat = new int[] {  2805,     0,     0,     0,     0 }, Multiplier = new int[] {  5000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 63, Suffix = "of Agility",            Stat = new int[] {  2802,     0,     0,     0,     0 }, Multiplier = new int[] {  5000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 64, Suffix = "of Power",              Stat = new int[] {  2825,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 65, Suffix = "of Magic",              Stat = new int[] {  2824,     0,     0,     0,     0 }, Multiplier = new int[] {  5850,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 66, Suffix = "of the Knight",         Stat = new int[] {  2803,  2813,  2824,     0,     0 }, Multiplier = new int[] {  7889,  5259,  6153,     0,     0 } },
new RandomSuffixDataType() { Id = 67, Suffix = "of the Seer",           Stat = new int[] {  2803,  2822,  2804,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 68, Suffix = "of the Bear",           Stat = new int[] {  2805,  2803,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 69, Suffix = "of the Eagle",          Stat = new int[] {  2803,  2804,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 70, Suffix = "of the Ancestor",       Stat = new int[] {  2805,  2822,  2803,     0,     0 }, Multiplier = new int[] {  7266,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 71, Suffix = "of the Bandit",         Stat = new int[] {  2802,  2803,  2822,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 72, Suffix = "of the Battle",         Stat = new int[] {  2805,  2803,  2822,     0,     0 }, Multiplier = new int[] {  7266,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 73, Suffix = "of the Elder",          Stat = new int[] {  2803,  2804,  2806,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 74, Suffix = "of the Beast",          Stat = new int[] {  3727,  2822,  2803,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 75, Suffix = "of the Champion",       Stat = new int[] {  2805,  2803,  2815,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 76, Suffix = "of the Grove",          Stat = new int[] {  2805,  2802,  2803,     0,     0 }, Multiplier = new int[] {  7266,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 77, Suffix = "of the Knight",         Stat = new int[] {  2803,  2813,  2824,     0,     0 }, Multiplier = new int[] {  5259,  5259,  6153,     0,     0 } },
new RandomSuffixDataType() { Id = 78, Suffix = "of the Monkey",         Stat = new int[] {  2802,  2803,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 79, Suffix = "of the Moon",           Stat = new int[] {  2804,  2803,  2806,     0,     0 }, Multiplier = new int[] {  5259,  3506,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 80, Suffix = "of the Wild",           Stat = new int[] {  2825,  2803,  2802,     0,     0 }, Multiplier = new int[] { 10518,  3506,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 81, Suffix = "of the Whale",          Stat = new int[] {  2803,  2806,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 82, Suffix = "of the Vision",         Stat = new int[] {  2824,  2804,  2803,     0,     0 }, Multiplier = new int[] {  8501,  4106,  2129,     0,     0 } },
new RandomSuffixDataType() { Id = 83, Suffix = "of the Sun",            Stat = new int[] {  2823,  2803,  2804,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 84, Suffix = "of Stamina",            Stat = new int[] {  2803,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 85, Suffix = "of the Sorcerer",       Stat = new int[] {  2803,  2804,  3726,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 86, Suffix = "of the Soldier",        Stat = new int[] {  2805,  2803,  2822,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 87, Suffix = "of the Shadow",         Stat = new int[] {  2825,  2802,  2803,     0,     0 }, Multiplier = new int[] { 14532,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 88, Suffix = "of the Foreseer",       Stat = new int[] {  2804,  3726,  2824,     0,     0 }, Multiplier = new int[] {  5259,  5259,  6153,     0,     0 } },
new RandomSuffixDataType() { Id = 89, Suffix = "of the Thief",          Stat = new int[] {  2803,  2825,  3726,     0,     0 }, Multiplier = new int[] {  7889, 10518,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 90, Suffix = "of the Necromancer",    Stat = new int[] {  2803,  3727,  2824,     0,     0 }, Multiplier = new int[] {  7889,  5259,  6153,     0,     0 } },
new RandomSuffixDataType() { Id = 91, Suffix = "of the Marksman",       Stat = new int[] {  2803,  2802,  3727,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 92, Suffix = "of the Squire",         Stat = new int[] {  2803,  3727,  2805,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 93, Suffix = "of Restoration",        Stat = new int[] {  2803,  2824,  2816,     0,     0 }, Multiplier = new int[] {  7889,  6153,  2103,     0,     0 } },
new RandomSuffixDataType() { Id = 94, Suffix = "",                      Stat = new int[] {  2802,     0,     0,     0,     0 }, Multiplier = new int[] {  9000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 95, Suffix = "",                      Stat = new int[] {  2805,     0,     0,     0,     0 }, Multiplier = new int[] {  9000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 96, Suffix = "",                      Stat = new int[] {  2803,     0,     0,     0,     0 }, Multiplier = new int[] { 13500,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 97, Suffix = "",                      Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] {  9000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 98, Suffix = "",                      Stat = new int[] {  2806,     0,     0,     0,     0 }, Multiplier = new int[] {  9000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 99, Suffix = "of Speed",              Stat = new int[] {  3726,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 100, Suffix = "of the Principle",     Stat = new int[] {  2803,  2805,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 101, Suffix = "of the Sentinel",      Stat = new int[] {  2803,  2805,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 102, Suffix = "of the Hero",          Stat = new int[] {  2803,  2805,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 103, Suffix = "of the Avatar",        Stat = new int[] {  2803,  2805,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 104, Suffix = "of the Embodiment",    Stat = new int[] {  2803,  2805,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 105, Suffix = "of the Guardian",      Stat = new int[] {  2803,  2805,  4059,  2826,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 106, Suffix = "of the Defender",      Stat = new int[] {  2803,  2805,  2826,  4060,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 107, Suffix = "of the Exemplar",      Stat = new int[] {  2803,  2805,  4058,  2815,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 108, Suffix = "of the Curator",       Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 109, Suffix = "of the Preserver",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 110, Suffix = "of the Elements",      Stat = new int[] {  2803,  2804,  3727,  2823,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 111, Suffix = "of the Paradigm",      Stat = new int[] {  2803,  2804,  3727,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 112, Suffix = "of the Pattern",       Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 113, Suffix = "of the Essence",       Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 114, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 115, Suffix = "of the Archetype",     Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 116, Suffix = "of the Manifestation", Stat = new int[] {  2803,  2802,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 117, Suffix = "of the Incarnation",   Stat = new int[] {  2803,  2802,  2823,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 118, Suffix = "of the Faultline",     Stat = new int[] {  2803,  2805,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 119, Suffix = "of the Ideal",         Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 120, Suffix = "of the Earthshaker",   Stat = new int[] {  2803,  2805,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 121, Suffix = "of the Landslide",     Stat = new int[] {  2803,  2805,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 122, Suffix = "of the Earthfall",     Stat = new int[] {  2803,  2805,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 123, Suffix = "of the Earthbreaker",  Stat = new int[] {  2803,  2805,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 124, Suffix = "of the Mountainbed",   Stat = new int[] {  2803,  2805,  4059,  4058,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 125, Suffix = "of the Bedrock",       Stat = new int[] {  2803,  2805,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 126, Suffix = "of the Substratum",    Stat = new int[] {  2803,  2805,  4058,  2815,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 127, Suffix = "of the Bouldercrag",   Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 128, Suffix = "of the Rockslab",      Stat = new int[] {  2803,  2805,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 129, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 130, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 131, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 132, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 133, Suffix = "of the Stormblast",    Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 134, Suffix = "of the Galeburst",     Stat = new int[] {  2803,  2802,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 135, Suffix = "of the Windflurry",    Stat = new int[] {  2803,  2802,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 136, Suffix = "of the Zephyr",        Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 137, Suffix = "of the Windstorm",     Stat = new int[] {  2803,  2802,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 138, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 139, Suffix = "of the Mercenary",     Stat = new int[] {  2803,  2805,  3726,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 140, Suffix = "of the Wraith",        Stat = new int[] {  2822,  2806,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 141, Suffix = "of the Wind",          Stat = new int[] {  2806,  3726,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 142, Suffix = "of the Master",        Stat = new int[] {  2806,  4059,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 143, Suffix = "of the Scorpion",      Stat = new int[] {  3726,  2822,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 144, Suffix = "of the Shark",         Stat = new int[] {  2822,  4059,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 145, Suffix = "of the Panther",       Stat = new int[] {  4059,  3726,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 146, Suffix = "Crit/Mastery (40/60)", Stat = new int[] {  2822,  4059,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 147, Suffix = "of the Shark",         Stat = new int[] {  2822,  4059,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 148, Suffix = "Crit/Spirit (40/60)",  Stat = new int[] {  2822,  2806,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 149, Suffix = "of the Scorpion",      Stat = new int[] {  3726,  2822,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 150, Suffix = "of the Panther",       Stat = new int[] {  4059,  3726,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 151, Suffix = "of the Wind",          Stat = new int[] {  2806,  3726,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 152, Suffix = "of the Master",        Stat = new int[] {  2806,  4059,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 153, Suffix = "of the Wraith",        Stat = new int[] {  3726,  4059,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 154, Suffix = "of the Shark",         Stat = new int[] {  2822,  4059,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 155, Suffix = "of the Scorpion",      Stat = new int[] {  3726,  2822,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 156, Suffix = "of the Wraith",        Stat = new int[] {  2822,  2806,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 157, Suffix = "of the Panther",       Stat = new int[] {  4059,  3726,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 158, Suffix = "of the Wind",          Stat = new int[] {  2806,  3726,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 159, Suffix = "of the Master",        Stat = new int[] {  2806,  4059,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 160, Suffix = "of the Mongoose",      Stat = new int[] {  3727,  3726,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 161, Suffix = "of Storms",            Stat = new int[] {  3727,  2822,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 162, Suffix = "of Flames",            Stat = new int[] {  3727,  4059,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 163, Suffix = "of the Mongoose",      Stat = new int[] {  3727,  3726,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 164, Suffix = "of Storms",            Stat = new int[] {  3727,  2822,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 165, Suffix = "of Flames",            Stat = new int[] {  3727,  4059,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 166, Suffix = "of the Mongoose",      Stat = new int[] {  3727,  4059,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 167, Suffix = "of Storms",            Stat = new int[] {  3727,  2822,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 168, Suffix = "of Flames",            Stat = new int[] {  3727,  4059,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 169, Suffix = "of the Landslide",     Stat = new int[] {  2803,  2805,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 170, Suffix = "of the Earthshaker",   Stat = new int[] {  2803,  2805,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 171, Suffix = "of the Earthfall",     Stat = new int[] {  2803,  2805,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 172, Suffix = "of the Faultline",     Stat = new int[] {  2803,  2805,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 173, Suffix = "of the Earthshaker",   Stat = new int[] {  2803,  2805,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 174, Suffix = "of the Landslide",     Stat = new int[] {  2803,  2805,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 175, Suffix = "of the Earthfall",     Stat = new int[] {  2803,  2805,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 176, Suffix = "of the Faultline",     Stat = new int[] {  2803,  2805,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 177, Suffix = "of the Bedrock",       Stat = new int[] {  2803,  2805,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 178, Suffix = "of the Bouldercrag",   Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 179, Suffix = "of the Rockslab",      Stat = new int[] {  2803,  2805,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 180, Suffix = "of the Bedrock",       Stat = new int[] {  2803,  2805,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 181, Suffix = "of the Bouldercrag",   Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 182, Suffix = "of the Rockslab",      Stat = new int[] {  2803,  2805,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 183, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 184, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 185, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 186, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 187, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 188, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 189, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 190, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 191, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 192, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 193, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 194, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 195, Suffix = "of the Stormblast",    Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 196, Suffix = "of the Windflurry",    Stat = new int[] {  2803,  2802,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 197, Suffix = "of the Windstorm",     Stat = new int[] {  2803,  2802,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 198, Suffix = "of the Zephyr",        Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 199, Suffix = "of the Stormblast",    Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 200, Suffix = "of the Windflurry",    Stat = new int[] {  2803,  2802,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 201, Suffix = "of the Windstorm",     Stat = new int[] {  2803,  2802,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 202, Suffix = "of the Zephyr",        Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 203, Suffix = "of the Earthshaker",   Stat = new int[] {  2803,  2805,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 204, Suffix = "of the Landslide",     Stat = new int[] {  2803,  2805,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 205, Suffix = "of the Earthfall",     Stat = new int[] {  2803,  2805,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 206, Suffix = "of the Faultline",     Stat = new int[] {  2803,  2805,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 207, Suffix = "of the Bedrock",       Stat = new int[] {  2803,  2805,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 208, Suffix = "of the Bouldercrag",   Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 209, Suffix = "of the Rockslab",      Stat = new int[] {  2803,  2805,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 210, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 211, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 212, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 213, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 214, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 215, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 216, Suffix = "of the Stormblast",    Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 217, Suffix = "of the Windflurry",    Stat = new int[] {  2803,  2802,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 218, Suffix = "of the Windstorm",     Stat = new int[] {  2803,  2802,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 219, Suffix = "of the Zephyr",        Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 220, Suffix = "of the Earthshaker",   Stat = new int[] {  2803,  2805,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 221, Suffix = "of the Landslide",     Stat = new int[] {  2803,  2805,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 222, Suffix = "of the Earthfall",     Stat = new int[] {  2803,  2805,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 223, Suffix = "of the Faultline",     Stat = new int[] {  2803,  2805,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 224, Suffix = "of the Bedrock",       Stat = new int[] {  2803,  2805,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 225, Suffix = "of the Bouldercrag",   Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 226, Suffix = "of the Rockslab",      Stat = new int[] {  2803,  2805,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 227, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 228, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 229, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 230, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 231, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 232, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 233, Suffix = "of the Stormblast",    Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 234, Suffix = "of the Windflurry",    Stat = new int[] {  2803,  2802,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 235, Suffix = "of the Windstorm",     Stat = new int[] {  2803,  2802,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 236, Suffix = "of the Zephyr",        Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } }, 
new RandomSuffixDataType() { Id = 260, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4580,  3165,  3165,     0 } },
new RandomSuffixDataType() { Id = 261, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  4580,  3165,  3165,     0 } },
new RandomSuffixDataType() { Id = 262, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4580,  3165,  3165,     0 } },
new RandomSuffixDataType() { Id = 263, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4580,  3165,  3165,     0 } },
new RandomSuffixDataType() { Id = 264, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4580,  3165,  3165,     0 } },
new RandomSuffixDataType() { Id = 265, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4490,  3125,  3125,     0 } },
new RandomSuffixDataType() { Id = 266, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  4490,  3125,  3125,     0 } },
new RandomSuffixDataType() { Id = 267, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4490,  3125,  3125,     0 } },
new RandomSuffixDataType() { Id = 268, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4490,  3125,  3125,     0 } },
new RandomSuffixDataType() { Id = 269, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4490,  3125,  3125,     0 } },
new RandomSuffixDataType() { Id = 270, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4490,  3125,  3125,     0 } },
new RandomSuffixDataType() { Id = 271, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4580,  3165,  3165,     0 } },
new RandomSuffixDataType() { Id = 272, Suffix = "of the Bedrock",       Stat = new int[] {  2803,  2805,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 273, Suffix = "of the Bouldercrag",   Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 274, Suffix = "of the Rockslab",      Stat = new int[] {  2803,  2805,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 275, Suffix = "of the Bedrock",       Stat = new int[] {  2803,  2805,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 276, Suffix = "of the Bouldercrag",   Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 277, Suffix = "of the Rockslab",      Stat = new int[] {  2803,  2805,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 278, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 279, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 280, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 281, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 282, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 283, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 284, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 285, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 286, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 287, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 288, Suffix = "of the Stormblast",    Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 289, Suffix = "of the Windflurry",    Stat = new int[] {  2803,  2802,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 290, Suffix = "of the Windstorm",     Stat = new int[] {  2803,  2802,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 291, Suffix = "of the Zephyr",        Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 292, Suffix = "of the Stormblast",    Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 293, Suffix = "of the Windflurry",    Stat = new int[] {  2803,  2802,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 294, Suffix = "of the Windstorm",     Stat = new int[] {  2803,  2802,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 295, Suffix = "of the Zephyr",        Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
        });
        private static List<int> allSuffixes;

        public static List<int> GetAllSuffixes()
        {
            if (allSuffixes == null)
            {
                List<int> list = new List<int>();
                foreach (var s in RandomSuffixData)
                {
                    if (s != null)
                    {
                        list.Add(s.Id);
                    }
                }
                allSuffixes = list;
            }
            return allSuffixes;
        }

        /// <summary>
        /// Gets the number of random stats.
        /// </summary>
        public static int GetStatCount(Item item, int id)
        {
            for (int i = 0; i < 5; i++)
            {
                if (RandomSuffixData[id].Stat[i] == 0)
                {
                    return i;
                }
            }
            return 5;
        }

        /// <summary>
        /// Gets the type of random stat at given index.
        /// </summary>
        public static AdditiveStat GetStat(Item item, int id, int index)
        {
            return StatFromEnchantmentId(RandomSuffixData[id].Stat[index]);
        }

        /// <summary>
        /// Gets the ammount of random stat at a given index.
        /// </summary>
        public static float GetStatValue(Item item, int id, int index)
        {
            if (item.ItemLevel < 277) return 0;
            int baseValue = RandomPropData[item.ItemLevel - 277, QualityIndex(item.Quality), SlotIndex(item.Slot)];
            int multiplier = RandomSuffixData[id].Multiplier[index];
            return (int)(multiplier / 10000.0 * baseValue);
        }

        /// <summary>
        /// Gets the ammount of random stat for a given stat.
        /// </summary>
        public static float GetStatValue(Item item, int id, AdditiveStat stat)
        {
            if (item.ItemLevel < 277) return 0;
            for (int i = 0; i < 5; i++)
            {
                int statId = RandomSuffixData[id].Stat[i];
                if (statId == 0)
                {
                    return 0;
                }
                if (stat == StatFromEnchantmentId(statId))
                {
                    int baseValue = RandomPropData[item.ItemLevel - 277, QualityIndex(item.Quality), SlotIndex(item.Slot)];
                    int multiplier = RandomSuffixData[id].Multiplier[i];
                    return (int)(multiplier / 10000.0 * baseValue);
                }
            }
            return 0;
        }

        /// <summary>
        /// Gets the suffix for given id.
        /// </summary>
        public static string GetSuffix(int id)
        {
            if (id < RandomSuffixData.Length)
                return RandomSuffixData[id].Suffix;
            else
                return "";
        }

        public static void AccumulateStats(Stats stats, Item item, int id)
        {
            //&UT& 
            // Should check ID to ensure it's not outside the array.
            if (item.ItemLevel < 277 
                || id < 0
                || id > RandomSuffixData.Length) return;
            for (int i = 0; i < 5; i++)
            {
                int statId = RandomSuffixData[id].Stat[i];
                if (statId == 0)
                {
                    return;
                }
                AdditiveStat stat = StatFromEnchantmentId(statId);
                int baseValue = RandomPropData[item.ItemLevel - 277, QualityIndex(item.Quality), SlotIndex(item.Slot)];
                int multiplier = RandomSuffixData[id].Multiplier[i];
                stats._rawAdditiveData[(int)stat] += (int)(multiplier / 10000.0 * baseValue);
            }
        }

        private static int QualityIndex(ItemQuality quality)
        {
            switch (quality)
            { 
                case ItemQuality.Epic:
                default:
                    return 0;
                case ItemQuality.Rare:
                    return 1;
                case ItemQuality.Uncommon:
                    return 2;
            }
        }

        private static int SlotIndex(ItemSlot slot)
        {
            switch (slot)
            {
                case ItemSlot.Head:
                case ItemSlot.Chest:
                case ItemSlot.Legs:
                case ItemSlot.TwoHand:
                    return 0;
                case ItemSlot.Shoulders:
                case ItemSlot.Hands:
                case ItemSlot.Waist:
                case ItemSlot.Feet:
                    return 1;
                case ItemSlot.Wrist:
                case ItemSlot.Neck:
                case ItemSlot.Back:
                case ItemSlot.Finger:
                case ItemSlot.OffHand:
                default:
                    return 2;
                case ItemSlot.MainHand:
                case ItemSlot.OneHand:
                    return 3;
                case ItemSlot.Ranged:
                    return 4;
            }
        }

        private static AdditiveStat StatFromEnchantmentId(int id)
        {
            switch (id)
            {
                case 2802:
                    return AdditiveStat.Agility;
                case 2803:
                    return AdditiveStat.Stamina;
                case 2804:
                    return AdditiveStat.Intellect;
                case 2805:
                    return AdditiveStat.Strength;
                case 2806:
                    return AdditiveStat.Spirit;
                case 2813:
                case 2815:
                    return AdditiveStat.DodgeRating;
                case 2816:
                    return AdditiveStat.Mp5;
                case 2822:
                case 2823:
                    return AdditiveStat.CritRating;
                case 2824:
                    return AdditiveStat.SpellPower;
                case 2825:
                    return AdditiveStat.AttackPower;
                case 2826:
                    return AdditiveStat.BlockRating;
                case 3726:
                    return AdditiveStat.HasteRating;
                case 3727:
                    return AdditiveStat.HitRating;
                case 4058:
                    return AdditiveStat.ExpertiseRating;
                case 4059:
                    return AdditiveStat.MasteryRating;
                case 4060:
                    return AdditiveStat.ParryRating;
            }
            return (AdditiveStat)(-1);
        }
    }
}
