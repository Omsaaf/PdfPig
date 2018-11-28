﻿namespace UglyToad.PdfPig.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class PigProductionHandbookTests
    {
        private static string GetFilename()
        {
            return IntegrationHelpers.GetDocumentPath("Pig Production Handbook.pdf");
        }

        [Fact]
        public void CanReadContent()
        {
            using (var document = PdfDocument.Open(GetFilename(), new ParsingOptions
            {
                UseLenientParsing = false
            }))
            {
                var page = document.GetPage(1);

                Assert.Contains("For the small holders at village level", page.Text);
            }
        }

        [Fact]
        public void Page1HasCorrectWords()
        {
            var expected = new List<string>
            {
                "European",
                "Comission",
                "Farmer's",
                "Hand",
                "Book",
                "on",
                "Pig",
                "Production",
                "(For",
                "the",
                "small",
                "holders",
                "at",
                "village",
                "level)",
                "GCP/NEP/065/EC",
                "Food",
                "and",
                "Agriculture",
                "Organization",
                "of",
                "the",
                "United",
                "Nations"
            };

            using (var document = PdfDocument.Open(GetFilename()))
            {
                var page = document.GetPage(1);

                var words = page.GetWords().ToList();

                Assert.Equal(expected, words.Select(x => x.Text));
            }
        }

        [Fact]
        public void Page4HasCorrectWords()
        {
            var expected = WordsPage4.Split(new[] {"\r", "\r\n", "\n", " "}, StringSplitOptions.RemoveEmptyEntries);
            using (var document = PdfDocument.Open(GetFilename()))
            {
                var page = document.GetPage(4);

                var words = page.GetWords().ToList();

                Assert.Equal(expected, words.Select(x => x.Text));
            }
        }

        [Fact]
        public void CanReadPage9()
        {
            using (var document = PdfDocument.Open(GetFilename(), new ParsingOptions
            {
                UseLenientParsing = false
            }))
            {
                var page = document.GetPage(9);

                Assert.Contains("BreedsNative breeds of pig can be found throughout the country. They are a small body size compared to other exotic and crosses pig types. There name varies from region to region, for example", page.Text);
            }
        }

        [Fact]
        public void HasCorrectNumberOfPages()
        {
            using (var document = PdfDocument.Open(GetFilename()))
            {
                Assert.Equal(86, document.NumberOfPages);
            }
        }

        [Fact]
        public void LettersHaveCorrectPosition()
        {
            using (var document = PdfDocument.Open(GetFilename()))
            {
                var page = document.GetPage(1);
                var letters = page.Letters;
                var positions = GetPdfBoxPositionData();

                for (var i = 0; i < letters.Count; i++)
                {
                    var letter = letters[i];
                    var position = positions[i];

                    position.AssertWithinTolerance(letter, page, false);
                }
            }
        }

        private static IReadOnlyList<AssertablePositionData> GetPdfBoxPositionData()
        {
            const string data = @"60.8189	495.7441	22.134	F	42.0	LAZFKS+MyriadPro-Bold	0.0
81.3149	495.7441	22.176	a	42.0	LAZFKS+MyriadPro-Bold	0.0
103.490906	495.7441	15.960	r	42.0	LAZFKS+MyriadPro-Bold	0.0
119.450905	495.7441	36.12	m	42.0	LAZFKS+MyriadPro-Bold	0.0
155.5709	495.7441	22.176	e	42.0	LAZFKS+MyriadPro-Bold	0.0
177.7469	495.7441	15.960	r	42.0	LAZFKS+MyriadPro-Bold	0.0
193.70691	495.7441	8.610	'	42.0	LAZFKS+MyriadPro-Bold	0.0
202.31691	495.7441	18.228	s	42.0	LAZFKS+MyriadPro-Bold	0.0
220.5449	495.7441	8.484	 	42.0	LAZFKS+MyriadPro-Bold	0.0
229.0289	495.7441	28.938	H	42.0	LAZFKS+MyriadPro-Bold	0.0
257.9669	495.7441	22.176	a	42.0	LAZFKS+MyriadPro-Bold	0.0
280.14288	495.7441	24.612	n	42.0	LAZFKS+MyriadPro-Bold	0.0
304.75488	495.7441	25.032	d	42.0	LAZFKS+MyriadPro-Bold	0.0
329.7869	495.7441	8.484	 	42.0	LAZFKS+MyriadPro-Bold	0.0
338.2709	495.7441	25.368002	B	42.0	LAZFKS+MyriadPro-Bold	0.0
363.89093	495.7441	24.234	o	42.0	LAZFKS+MyriadPro-Bold	0.0
388.25095	495.7441	24.234	o	42.0	LAZFKS+MyriadPro-Bold	0.0
412.48495	495.7441	22.764002	k	42.0	LAZFKS+MyriadPro-Bold	0.0
435.24896	495.7441	8.484	 	42.0	LAZFKS+MyriadPro-Bold	0.0
57.1844	453.7441	27.626762	o	47.88	LAZFKS+MyriadPro-Bold	0.0
84.81116	453.7441	28.057682	n	47.88	LAZFKS+MyriadPro-Bold	0.0
112.868835	453.7441	9.671761	 	47.88	LAZFKS+MyriadPro-Bold	0.0
122.540596	453.7441	27.818281	P	47.88	LAZFKS+MyriadPro-Bold	0.0
149.92795	453.7441	13.119122	i	47.88	LAZFKS+MyriadPro-Bold	0.0
163.04707	453.7441	28.009802	g	47.88	LAZFKS+MyriadPro-Bold	0.0
191.05687	453.7441	9.671761	 	47.88	LAZFKS+MyriadPro-Bold	0.0
200.72864	453.7441	27.818281	P	47.88	LAZFKS+MyriadPro-Bold	0.0
228.116	453.7441	18.1944	r	47.88	LAZFKS+MyriadPro-Bold	0.0
245.92735	453.7441	27.626762	o	47.88	LAZFKS+MyriadPro-Bold	0.0
273.69775	453.7441	28.536482	d	47.88	LAZFKS+MyriadPro-Bold	0.0
302.23422	453.7441	27.914042	u	47.88	LAZFKS+MyriadPro-Bold	0.0
330.14825	453.7441	21.593882	c	47.88	LAZFKS+MyriadPro-Bold	0.0
352.5082	453.7441	17.57196	t	47.88	LAZFKS+MyriadPro-Bold	0.0
370.08017	453.7441	13.119122	i	47.88	LAZFKS+MyriadPro-Bold	0.0
383.19928	453.7441	27.626762	o	47.88	LAZFKS+MyriadPro-Bold	0.0
410.82605	453.7441	28.057682	n	47.88	LAZFKS+MyriadPro-Bold	0.0
57.3618	418.1143	7.0886397	(	24.96	GDKBSC+MyriadPro-Regular	0.0
64.45044	418.1143	12.15552	F	24.96	GDKBSC+MyriadPro-Regular	0.0
75.8322	418.1143	13.70304	o	24.96	GDKBSC+MyriadPro-Regular	0.0
89.53524	418.1143	8.161921	r	24.96	GDKBSC+MyriadPro-Regular	0.0
97.69716	418.1143	5.29152	 	24.96	GDKBSC+MyriadPro-Regular	0.0
102.98868	418.1143	8.261761	t	24.96	GDKBSC+MyriadPro-Regular	0.0
111.250435	418.1143	13.852799	h	24.96	GDKBSC+MyriadPro-Regular	0.0
125.10323	418.1143	12.504961	e	24.96	GDKBSC+MyriadPro-Regular	0.0
137.60818	418.1143	5.29152	 	24.96	GDKBSC+MyriadPro-Regular	0.0
142.8997	418.1143	9.88416	s	24.96	GDKBSC+MyriadPro-Regular	0.0
152.78386	418.1143	20.81664	m	24.96	GDKBSC+MyriadPro-Regular	0.0
173.6005	418.1143	12.03072	a	24.96	GDKBSC+MyriadPro-Regular	0.0
185.63121	418.1143	5.89056	l	24.96	GDKBSC+MyriadPro-Regular	0.0
191.52177	418.1143	5.89056	l	24.96	GDKBSC+MyriadPro-Regular	0.0
197.41234	418.1143	5.29152	 	24.96	GDKBSC+MyriadPro-Regular	0.0
202.70386	418.1143	13.852799	h	24.96	GDKBSC+MyriadPro-Regular	0.0
216.55666	418.1143	13.70304	o	24.96	GDKBSC+MyriadPro-Regular	0.0
230.25969	418.1143	5.89056	l	24.96	GDKBSC+MyriadPro-Regular	0.0
236.15025	418.1143	14.077439	d	24.96	GDKBSC+MyriadPro-Regular	0.0
250.22769	418.1143	12.504961	e	24.96	GDKBSC+MyriadPro-Regular	0.0
262.73267	418.1143	8.161921	r	24.96	GDKBSC+MyriadPro-Regular	0.0
270.8946	418.1143	9.88416	s	24.96	GDKBSC+MyriadPro-Regular	0.0
280.77875	418.1143	5.29152	 	24.96	GDKBSC+MyriadPro-Regular	0.0
286.07028	418.1143	12.03072	a	24.96	GDKBSC+MyriadPro-Regular	0.0
298.00116	418.1143	8.261761	t	24.96	GDKBSC+MyriadPro-Regular	0.0
306.2629	418.1143	5.29152	 	24.96	GDKBSC+MyriadPro-Regular	0.0
311.55444	418.1143	12.00576	v	24.96	GDKBSC+MyriadPro-Regular	0.0
323.5602	418.1143	5.84064	i	24.96	GDKBSC+MyriadPro-Regular	0.0
329.40085	418.1143	5.89056	l	24.96	GDKBSC+MyriadPro-Regular	0.0
335.2914	418.1143	5.89056	l	24.96	GDKBSC+MyriadPro-Regular	0.0
341.18198	418.1143	12.03072	a	24.96	GDKBSC+MyriadPro-Regular	0.0
353.2127	418.1143	13.95264	g	24.96	GDKBSC+MyriadPro-Regular	0.0
367.16534	418.1143	12.504961	e	24.96	GDKBSC+MyriadPro-Regular	0.0
379.6703	418.1143	5.29152	 	24.96	GDKBSC+MyriadPro-Regular	0.0
384.96182	418.1143	5.89056	l	24.96	GDKBSC+MyriadPro-Regular	0.0
390.8524	418.1143	12.504961	e	24.96	GDKBSC+MyriadPro-Regular	0.0
403.35736	418.1143	12.00576	v	24.96	GDKBSC+MyriadPro-Regular	0.0
415.11353	418.1143	12.504961	e	24.96	GDKBSC+MyriadPro-Regular	0.0
427.61847	418.1143	5.89056	l	24.96	GDKBSC+MyriadPro-Regular	0.0
433.50903	418.1143	7.0886397	)	24.96	GDKBSC+MyriadPro-Regular	0.0
212.6322	63.3661	6.46	G	10.0	GDKBSC+MyriadPro-Regular	0.0
219.09221	63.3661	5.8	C	10.0	GDKBSC+MyriadPro-Regular	0.0
224.89221	63.3661	5.32	P	10.0	GDKBSC+MyriadPro-Regular	0.0
230.21222	63.3661	3.4300003	/	10.0	GDKBSC+MyriadPro-Regular	0.0
233.64221	63.3661	6.5800004	N	10.0	GDKBSC+MyriadPro-Regular	0.0
240.22221	63.3661	4.92	E	10.0	GDKBSC+MyriadPro-Regular	0.0
245.14221	63.3661	5.32	P	10.0	GDKBSC+MyriadPro-Regular	0.0
250.46222	63.3661	3.4300003	/	10.0	GDKBSC+MyriadPro-Regular	0.0
253.89221	63.3661	5.13	0	10.0	GDKBSC+MyriadPro-Regular	0.0
259.02222	63.3661	5.13	6	10.0	GDKBSC+MyriadPro-Regular	0.0
264.15222	63.3661	5.13	5	10.0	GDKBSC+MyriadPro-Regular	0.0
269.28223	63.3661	3.4300003	/	10.0	GDKBSC+MyriadPro-Regular	0.0
272.71222	63.3661	4.92	E	10.0	GDKBSC+MyriadPro-Regular	0.0
277.63223	63.3661	5.8	C	10.0	GDKBSC+MyriadPro-Regular	0.0
71.9488	43.6968	7.3780003	F	14.0	LAZFKS+MyriadPro-Bold	0.0
78.9628	43.6968	8.078	o	14.0	LAZFKS+MyriadPro-Bold	0.0
87.0828	43.6968	8.078	o	14.0	LAZFKS+MyriadPro-Bold	0.0
95.202805	43.6968	8.344	d	14.0	LAZFKS+MyriadPro-Bold	0.0
103.54681	43.6968	2.828	 	14.0	LAZFKS+MyriadPro-Bold	0.0
106.37481	43.6968	7.392	a	14.0	LAZFKS+MyriadPro-Bold	0.0
113.76681	43.6968	8.204	n	14.0	LAZFKS+MyriadPro-Bold	0.0
121.97081	43.6968	8.344	d	14.0	LAZFKS+MyriadPro-Bold	0.0
130.3148	43.6968	2.828	 	14.0	LAZFKS+MyriadPro-Bold	0.0
133.1428	43.6968	9.184	A	14.0	LAZFKS+MyriadPro-Bold	0.0
142.13081	43.6968	8.190	g	14.0	LAZFKS+MyriadPro-Bold	0.0
150.32082	43.6968	5.32	r	14.0	LAZFKS+MyriadPro-Bold	0.0
155.64081	43.6968	3.8360002	i	14.0	LAZFKS+MyriadPro-Bold	0.0
159.4768	43.6968	6.3140006	c	14.0	LAZFKS+MyriadPro-Bold	0.0
165.7908	43.6968	8.162	u	14.0	LAZFKS+MyriadPro-Bold	0.0
173.9528	43.6968	3.8500	l	14.0	LAZFKS+MyriadPro-Bold	0.0
177.80281	43.6968	5.138	t	14.0	LAZFKS+MyriadPro-Bold	0.0
182.94081	43.6968	8.162	u	14.0	LAZFKS+MyriadPro-Bold	0.0
191.10281	43.6968	5.32	r	14.0	LAZFKS+MyriadPro-Bold	0.0
196.31082	43.6968	7.392	e	14.0	LAZFKS+MyriadPro-Bold	0.0
203.70282	43.6968	2.828	 	14.0	LAZFKS+MyriadPro-Bold	0.0
206.53082	43.6968	10.038	O	14.0	LAZFKS+MyriadPro-Bold	0.0
216.62482	43.6968	5.32	r	14.0	LAZFKS+MyriadPro-Bold	0.0
221.83282	43.6968	8.190	g	14.0	LAZFKS+MyriadPro-Bold	0.0
230.02283	43.6968	7.392	a	14.0	LAZFKS+MyriadPro-Bold	0.0
237.41483	43.6968	8.204	n	14.0	LAZFKS+MyriadPro-Bold	0.0
245.61882	43.6968	3.8360002	i	14.0	LAZFKS+MyriadPro-Bold	0.0
249.45482	43.6968	6.566	z	14.0	LAZFKS+MyriadPro-Bold	0.0
256.0208	43.6968	7.392	a	14.0	LAZFKS+MyriadPro-Bold	0.0
263.3008	43.6968	5.138	t	14.0	LAZFKS+MyriadPro-Bold	0.0
268.4388	43.6968	3.8360002	i	14.0	LAZFKS+MyriadPro-Bold	0.0
272.2748	43.6968	8.078	o	14.0	LAZFKS+MyriadPro-Bold	0.0
280.3528	43.6968	8.204	n	14.0	LAZFKS+MyriadPro-Bold	0.0
288.55682	43.6968	2.828	 	14.0	LAZFKS+MyriadPro-Bold	0.0
291.38483	43.6968	8.078	o	14.0	LAZFKS+MyriadPro-Bold	0.0
299.46283	43.6968	4.774	f	14.0	LAZFKS+MyriadPro-Bold	0.0
304.23682	43.6968	2.828	 	14.0	LAZFKS+MyriadPro-Bold	0.0
307.06482	43.6968	5.138	t	14.0	LAZFKS+MyriadPro-Bold	0.0
312.20282	43.6968	8.204	h	14.0	LAZFKS+MyriadPro-Bold	0.0
320.40683	43.6968	7.392	e	14.0	LAZFKS+MyriadPro-Bold	0.0
327.79883	43.6968	2.828	 	14.0	LAZFKS+MyriadPro-Bold	0.0
330.62683	43.6968	9.548	U	14.0	LAZFKS+MyriadPro-Bold	0.0
340.17484	43.6968	8.204	n	14.0	LAZFKS+MyriadPro-Bold	0.0
348.37885	43.6968	3.8360002	i	14.0	LAZFKS+MyriadPro-Bold	0.0
352.21484	43.6968	5.138	t	14.0	LAZFKS+MyriadPro-Bold	0.0
357.28284	43.6968	7.392	e	14.0	LAZFKS+MyriadPro-Bold	0.0
364.67484	43.6968	8.344	d	14.0	LAZFKS+MyriadPro-Bold	0.0
373.01883	43.6968	2.828	 	14.0	LAZFKS+MyriadPro-Bold	0.0
375.84683	43.6968	9.660	N	14.0	LAZFKS+MyriadPro-Bold	0.0
385.50684	43.6968	7.392	a	14.0	LAZFKS+MyriadPro-Bold	0.0
392.78683	43.6968	5.138	t	14.0	LAZFKS+MyriadPro-Bold	0.0
397.92484	43.6968	3.8360002	i	14.0	LAZFKS+MyriadPro-Bold	0.0
401.76083	43.6968	8.078	o	14.0	LAZFKS+MyriadPro-Bold	0.0
409.83884	43.6968	8.204	n	14.0	LAZFKS+MyriadPro-Bold	0.0
418.04285	43.6968	6.076	s	14.0	LAZFKS+MyriadPro-Bold	0.0
375.2348	587.6642	3.6815999	E	10.4	REXPQG+MyriadPro-Cond	0.0
378.7708	587.6642	4.1392	u	10.4	REXPQG+MyriadPro-Cond	0.0
382.91	587.6642	2.5792	r	10.4	REXPQG+MyriadPro-Cond	0.0
385.3436	587.6642	3.9832	o	10.4	REXPQG+MyriadPro-Cond	0.0
389.32678	587.6642	4.1496	p	10.4	REXPQG+MyriadPro-Cond	0.0
393.47638	587.6642	3.7752	e	10.4	REXPQG+MyriadPro-Cond	0.0
397.2516	587.6642	3.7856002	a	10.4	REXPQG+MyriadPro-Cond	0.0
401.0372	587.6642	4.2120004	n	10.4	REXPQG+MyriadPro-Cond	0.0
405.2492	587.6642	1.6328	 	10.4	REXPQG+MyriadPro-Cond	0.0
406.88202	587.6642	3.8792002	C	10.4	REXPQG+MyriadPro-Cond	0.0
410.57404	587.6642	3.9832	o	10.4	REXPQG+MyriadPro-Cond	0.0
414.55722	587.6642	6.448	m	10.4	REXPQG+MyriadPro-Cond	0.0
421.00522	587.6642	1.9136	i	10.4	REXPQG+MyriadPro-Cond	0.0
422.91882	587.6642	3.0056	s	10.4	REXPQG+MyriadPro-Cond	0.0
425.92444	587.6642	3.0056	s	10.4	REXPQG+MyriadPro-Cond	0.0
428.93005	587.6642	1.9136	i	10.4	REXPQG+MyriadPro-Cond	0.0
430.84366	587.6642	3.9832	o	10.4	REXPQG+MyriadPro-Cond	0.0
434.82684	587.6642	4.2120004	n	10.4	REXPQG+MyriadPro-Cond	0.0";

            var result = data.Split(new[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(AssertablePositionData.Parse)
                .ToList();

            return result;
        }

        private const string WordsPage4 = @"Disclaimer
The designations employed end the presentation of the material in this information
product do not imply the expression of any opinion whatsoever on the part of the
Food and Agriculture Organization of the United Nations (FAO) concerning the
legal or development status of any country, territory, city or area of its authorities,
or concerning the delimitation of its frontiers or boundaries. The mention of
specific companies or products of manufacturers, whether or not these have been
patented, does not imply that these have been endorsed or recommended by FAO
in preference to others of similar nature that are not mentioned.
The views expressed in this publication are those of the author(s) and do not
necessarily reflects the views of FAO.
All rights reserved. Reproduction and dissemination of materials in this information
product for educational or other non-commercial purposes are authorized without
any prior written permission from the copyright holders provided the source is
fully acknowledged. Reproduction in this information product for resale or other
commercial purposes is prohibited without written permission of the copyright
holders. Applications for such permission should be addressed to: Chief, Electronic
Publishing Policy and Support Branch Communication Division, FAO, Viale delle
Terme di Caracalla, 00153 Rome, Italy or by e-mail to: copyright@fao.org
FAO 2009
design&print: wps, eMail: printnepal@gmail.com";
    }
}