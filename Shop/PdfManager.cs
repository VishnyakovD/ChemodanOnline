using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Shop.Models;

namespace Shop
{
    public class PdfManager
    {
        public PdfManager()
        {

        }
        public MemoryStream BuildDocument(EditOrderModel order)
        {
            return RussianPDF(order);
        }

        public MemoryStream RussianPDF(EditOrderModel order)
        {
            try
            {
                MemoryStream workStream = new MemoryStream();
                Document document = new Document(PageSize.A4, 30, 30, 30, 30);
                PdfWriter.GetInstance(document, workStream).CloseStream = false;

                string sylfaenpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\arial.ttf";
                var sylfaen = BaseFont.CreateFont(sylfaenpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var head = new Font(sylfaen, 8f, Font.NORMAL, BaseColor.BLACK);
                var normal = new Font(sylfaen, 8f, Font.NORMAL, BaseColor.BLACK);
                var underline = new Font(sylfaen, 10f, Font.UNDERLINE, BaseColor.BLACK);
                var bold = new Font(sylfaen, 9f, Font.BOLD, BaseColor.BLACK);

                document.Open();

                var orderNumber = ((order.Order.OrderPrefix > 0)
                    ? $"{order.Order.OrderNumber}-{order.Order.OrderPrefix}"
                    : $"{order.Order.OrderNumber}");

                var zalog = order.Order.OrderProducts.Sum(item => item.FullPrice);

                document.Add(new Paragraph($"                                                                                                        Договір найму №{orderNumber}", head) );

                document.Add(new Paragraph(" ", normal));
                document.Add(new Paragraph($"м. Київ                                                                                                                                                                                                     " + DateTime.Now.Date.ToString("dd MMMM yyyy") +"р.",normal));
                document.Add(new Paragraph(" ", normal));
                //document.Add(new Paragraph(" ", normal));
                //document.Add(new Paragraph(" ", normal));
                document.Add(new Paragraph($"Фізична особа підприємець Глущенко Iнна Миколаївна з однієї сторони (далі в тексті – Наймодавець) та {order.Order.ClientLastName} {order.Order.ClientFirstName} з другої сторони (далі в тексті – Наймач), разом іменовані – Сторони, уклали цей договір про наступне:", normal));
                //document.Add(new Paragraph(" ", normal));
                document.Add(new Paragraph("1.  Даний Договір є договором приєднання у контексті статті 634 Цивільного кодексу України.", normal));
                document.Add(new Paragraph("2.  У цьому Договорі під терміном “Сайт” розуміється розташований в мережі Інтернет сайт за адресою https://chemodan.online.", normal));
                document.Add(new Paragraph("3.  Терміни та поняття, значення яких не розкрито в цьому Договорі, розуміються Сторонами відповідно чинного законодавства України.", normal));
                document.Add(new Paragraph("4.  Наймодавець зобов'язується надавати Наймачу за плату в тимчасове користування валізи/дорожні сумки та/або чохла на валізу/дорожню сумку (надалі за текстом – \"Майно\") у порядку та на умовах встановлених в цьому Договорі.", normal));
                document.Add(new Paragraph("5.  Прийняття умов Договору та його укладення є здійснення Наймачем дій, визначених п. 8 цього Договору.", normal));
                document.Add(new Paragraph("6.  Строк прийняття пропозиції Наймачем від Наймодавця є необмеженим. Прийняття умов Договору означає повну і беззаперечну згоду Наймача зі всіма умовами Договору без будь-яких виключень і/або обмежень і рівносильний укладенню письмового Договору відповідно до положень законодавства України. (ч. 2 ст. 642 ЦК України).", normal));
                document.Add(new Paragraph("7.  Місцем укладення Договору є місцезнаходження Наймодавця. Місцем виконання Договору є місце передачі в найм Майна. ", normal));
                document.Add(new Paragraph("8.  Для укладення Договору в простій письмовій формі Наймачу необхідно підписати Договір зміст якого викладеній на папері і який вважається укладеним з моменту його підписання обома Сторонами.", normal));
                document.Add(new Paragraph($"9.  Майно передається Наймодавцем Наймачу не пізніше {order.Order.From.ToString("dd.MM.yyyy")}, а Наймач повинен прийняти таке Майно. Претензії щодо невідповідності Майна його опису на сайті та його придатності для користування приймаються виключно в день отримання Наймачем Майна (у випадку якщо воно отримане шляхом доставки кур’єром), а у випадку отримання Майна у пункті прийому-видачі за адресою: м. Київ, пр. Перемоги, 67, корпус I, офіс 213 - тільки до моменту підписання акту прийому-передачі Майна. Разом із Майном Наймачу передається квитанція.", normal)); 
                document.Add(new Paragraph("10.  Документом, який підтверджує передачу або повернення Майна із зазначенням стану Майна є акт прийому - передачі.", normal));
                document.Add(new Paragraph("11.  Майно, надане за даним Договором повинно використовуватися Наймачем виключно за його цільовим призначенням. Враховуючи особливості використання Майна, Наймач зобов’язується у процесі використання здійснювати всі можливі дії для його збереження у стані в якому Майно було ним отримано від Наймодавця.", normal));
                document.Add(new Paragraph("12.  Наймач зобов`язаний повернути Майно Наймодавцю не пізніше останнього дня користування Майном згідно строків встановлених у п. 14 Договору та у стані, який є не гірший (з урахуванням нормального зносу) ніж на момент отримання Майна Наймачем.", normal));
                document.Add(new Paragraph("13.  З метою забезпечення виконання Наймачем зобов`язань передбаченого п. 11 та п.12 цього Договору Наймач повинен сплатити Наймодавцю грошову заставу у розмірі повної вартості Майна. Наймач не має право користуватися сплаченою грошовою заставою до моменту повернення Майна. Сторони погоджуються, що у випадку порушень Наймачем зобов’язань встановлених в п. 11 та п. 12 Договору, Наймодавець без будь-яких додаткових погоджень із Наймачем або звернень до суду чи інших осіб набуває право власності на заставу у розмірі який встановлюється із врахуванням п. 14, п. 15, та п. 16 цього Договору.", normal));
                document.Add(new Paragraph("Грошова застава повертається Наймодавцем Наймачу за вирахуванням тієї частини, на яку Наймодавець набув право власності. В результаті порушень Наймачем зобов’язань передбачених п. 11 та п.12 Договору після моменту повернення Майна (якщо таке повернення було здійснено Наймачем в пункт прийому-видачі Майна) Наймодавцю з грошової застави окрім вирахування тієї частини, на яку Наймодавець набув право власності також вираховується штраф у розмірі вартості найму Майна за кожну добу користування Майном понад строки визначені в Договорі та/або відшкодування за пошкодження Майна в розмірі передбачених п.15.", normal));
                decimal days = (decimal)(Math.Round((order.Order.To - order.Order.From).TotalDays));
                document.Add(new Paragraph($"14.  Строк користування Майном становить {days} календарних днів починаючи з {order.Order.From.ToString("dd.MM.yyyy")}р. по {order.Order.To.ToString("dd.MM.yyyy")}р. включно.", normal));
                document.Add(new Paragraph("      14.1.  Майно, що передається Наймачу:", normal));
                document.Add(new Paragraph(" ", normal));

                PdfPTable table = new PdfPTable(4);

                table.AddCell(new PdfPCell(new Phrase("Товарний код", normal)) {HorizontalAlignment = 1, MinimumHeight = 30, VerticalAlignment = Element.ALIGN_MIDDLE });
                table.AddCell(new PdfPCell(new Phrase("Вартість найму Майна за добу, грн.", normal)) {HorizontalAlignment = 1, MinimumHeight = 30, VerticalAlignment = Element.ALIGN_MIDDLE });
                table.AddCell(new PdfPCell(new Phrase("Вартість найму за весь період, грн.", normal)) {HorizontalAlignment = 1, MinimumHeight = 30, VerticalAlignment = Element.ALIGN_MIDDLE });
                table.AddCell(new PdfPCell(new Phrase("Заставна вартість, грн.", normal)) {HorizontalAlignment = 1, MinimumHeight = 30, VerticalAlignment = Element.ALIGN_MIDDLE });
                

                foreach (var item in order.Order.OrderProducts)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.Code, normal)) { HorizontalAlignment = 1, MinimumHeight = 25,VerticalAlignment = Element.ALIGN_MIDDLE });
                    table.AddCell(new PdfPCell(new Phrase(item.PriceDay.ToString("f2"), normal)) { HorizontalAlignment = 1, MinimumHeight = 25, VerticalAlignment = Element.ALIGN_MIDDLE });
                    table.AddCell(new PdfPCell(new Phrase((item.PriceDay*days).ToString("f2"), normal)) { HorizontalAlignment = 1, MinimumHeight = 25, VerticalAlignment = Element.ALIGN_MIDDLE });
                    table.AddCell(new PdfPCell(new Phrase(item.FullPrice.ToString("f2"), normal)) { HorizontalAlignment = 1, MinimumHeight = 25, VerticalAlignment = Element.ALIGN_MIDDLE });
                }

                document.Add(table);

                //document.Add(new Paragraph(" ", normal));

                document.Add(new Paragraph($"      14.2.  Загальна вартість прокату Майна за період згідно з п. 14 становить  {(order.Order.OrderProducts.Sum(item => item.PriceDay) * days).ToString("F2")} грн.", normal));
                document.Add(new Paragraph($"      14.3.  Повна вартість Майна (сума застави), що передається Наймачем Наймодавцеві, складає: {zalog.ToString("F2")} (______________________________) грн.", normal));
                document.Add(new Paragraph("       14.4.  Взаєморозрахунки між Сторонами можуть проводитися в готівковій або безготівковій формі. Наймач сплачує Наймодавцю в момент повернення Майна штраф у розмірі вартості найму Майна за кожну добу користування Майном понад строки визначені в Договорі.", normal));
                document.Add(new Paragraph("  15.  У випадку пошкодження Майна/зміни його стану незалежно від вини Наймача останній несе повну матеріальну відповідальність перед Наймодавцем за такі пошкодження/зміни враховуючи наступне:", normal));
                document.Add(new Paragraph("      15.1.  Пошкодження/зміна стану Майна, що виключають його використання в подальшому. До таких пошкоджень або змін відносяться поріз тканини, тріщина пластику. У випадку наявності таких пошкоджень/змін Майна Наймач  зобов`язаний сплатити Наймачу повну вартість Майна згідно із п. 14.3 в момент повернення Майна Наймодавцю.", normal));
                document.Add(new Paragraph("      15.2.  Пошкодження/зміна стану Майна, механізмів, вузлів або частин Майна після ремонту/чистки яких подальше використання Майна можливе і це не вплине на подальший строк його використання. У випадку наявності таких пошкоджень/змін стану Майна Наймач повинен сплатити Наймодавцю в момент повернення Майна вартість такого ремонту, розмір якого встановлюється згідно прейскуранту, який розміщено в пункті прийому-видачі.", normal));
                document.Add(new Paragraph("      15.3.  Пошкодження/зміна стану Майна, механізмів, вузлів або частин Майна після ремонту/чистки яких подальше використання Майна можливе але це вплине на якість/строк користування (наприклад, набуття неприйнятного запаху, деформація пластику  без тріщини). У випадку наявності таких пошкоджень/змін стану Майна Наймач повинен сплатити Наймодавцю вартість такого ремонту в момент повернення Майна, розмір якого встановлюється згідно прейскуранту, який розміщено в пункті прийому-видачі та додатково сплатити штраф у розмірі 10% повної вартості Майна яка встановлена п. 14.3. Договору.", normal));
                document.Add(new Paragraph("  16.  Повернення Майна Наймачем Наймодавцю здійснюється в пункті прийому-видачі за адресою: м. Київ, пр. Перемоги, 67, корпус “I”, офіс 213. У випадку наявності пошкоджень/зміни стану Майна Наймодавець в присутності Наймача оформлює акт про наявність пошкоджень/змін стану Майна. На підставі зазначеного акту та відповідного прейскуранту встановлюється вартість ремонту/чистки Майна. У випадку втрати чохла, Наймач сплачує грошове відшкодування в розмірі 250 грн.", normal));
                document.Add(new Paragraph("  17.  Будь-які спори за цим Договором вирішуються шляхом переговорів або в суді відповідно до чинного законодавства України.", normal));
                document.Add(new Paragraph("  18.  Сторони зобов’язуються при укладенні, виконанні та після припинення цього Договору дотримуватися вимог законодавчих та інших нормативно-правових актів України у сфері захисту персональних даних, в т.ч. щодо їх отримання, обробки, зберігання, якщо інше не врегульоване письмовою домовленістю Сторін.", normal));
                document.Add(new Paragraph("  19.  Сторони усвідомлюють, що в рамках виконання зобов’язань за цим Договором вони можуть обмінюватись документами або іншими даними, які містять відомості, що належать до персональних даних фізичних осіб (підписанти, відповідальні/контактні особи тощо). При цьому уповноважені представники Сторін (підписанти), укладаючи цей Договір, по відношенню до персональних даних зобов’язуються:", normal));
                document.Add(new Paragraph("      19.1.  гарантувати отримання згоди на обробку вказаних даних від суб’єктів персональних даних - винятково відповідно до мети, визначеної предметом та зобов’язаннями Сторін за цим Договором, гарантувати повідомлення суб’єктів персональних даних про їх включення до відповідних баз та повідомлення таких осіб про їхні права, визначені законодавством.", normal));
                document.Add(new Paragraph("      19.2.  надавати свою згоду, шляхом підписання цього Договору, на обробку власних персональних даних та вважатися повідомленим про включення його персональних даних до відповідної бази даних іншої Сторони та повідомленим про права, визначені законодавством.", normal));
                document.Add(new Paragraph("  20.  Будь-які персональні дані, що передаються чи можуть передаватись за цим Договором, становитимуть конфіденційну інформацію, що не підлягає розголошенню/передачі у будь-якому вигляді, окрім випадків, прямо передбачених законодавством України. Про всі випадки розголошення/передачі персональних даних за цим Договором Сторони негайно інформують одна одну у письмовому вигляді.", normal));
                document.Add(new Paragraph("  21.  Договір укладено на невизначений строк але у будь-якому разі до повного виконання сторонами своїх обов’язків.", normal));
                document.Add(new Paragraph("  22.  Договір може бути розірвано виключно за взаємною згодою Сторін шляхом підписання про це угоди на паперовому носії.", normal));
                document.Add(new Paragraph("  ", normal));

                float[] widths = new float[] { 20f, 70f, 30, 20f, 70f };
                PdfPTable table1 = new PdfPTable(widths);
                table1.WidthPercentage = 100;

                table1.AddCell(new PdfPCell(new Phrase("Наймодавець:", bold)) {MinimumHeight = 16, Colspan = 2,Border =0 });
                table1.AddCell(new PdfPCell(new Phrase("   ", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("Наймач: ", bold)) { MinimumHeight = 16, Colspan = 2, Border = 0});

                table1.AddCell(new PdfPCell(new Phrase("ФОП:", normal)) {MinimumHeight = 16, Border = 0});
                table1.AddCell(new PdfPCell(new Phrase("Глущенко І.М.", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("   ", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("ПІБ:", normal)) { MinimumHeight = 16, Border = 0});
                table1.AddCell(new PdfPCell(new Phrase($"{order.Order.ClientLastName} {order.Order.ClientFirstName}", normal)) { MinimumHeight = 16, Border = 0 });

                table1.AddCell(new PdfPCell(new Phrase("Код:", normal)) { MinimumHeight = 16, Border = 0});
                table1.AddCell(new PdfPCell(new Phrase("3108104242", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("   ", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("   ", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("   ", normal)) { MinimumHeight = 16, Border = 0 });

                table1.AddCell(new PdfPCell(new Phrase("Р/р", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("26003125296900", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("   ", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("   ", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("   ", normal)) { MinimumHeight = 16, Border = 0 });

                table1.AddCell(new PdfPCell(new Phrase("Адреса", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("пр. Перемоги, 67, корпус «I», офіс 213", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("   ", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("Адреса", normal)) { MinimumHeight = 16, Border = 0 });

                string flat = (string.IsNullOrEmpty(order.Order.Flat) ? "" : ", кв "+order.Order.Flat);
                string address =!string.IsNullOrEmpty(order.Order.City)?$"{order.Order.City}, {order.Order.TypeStreet} {order.Order.Street}, дом {order.Order.Home}{flat}":"";
                table1.AddCell(new PdfPCell(new Phrase($"{address}", normal)) { MinimumHeight = 16, Border = 0 });

                table1.AddCell(new PdfPCell(new Phrase("Email", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("info@chemodan.online", normal)) { MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase(" ", normal)) {  MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("Email", normal)) {  MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase($"{order.Order.ClientEmail}", normal)) {  MinimumHeight = 16, Border = 0 });

                table1.AddCell(new PdfPCell(new Phrase("Телефон", normal)) {  MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("‎+38 (093) 444 50 54", normal)) {  MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("", normal)) {  MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("Телефон", normal)) {  MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase($"{order.Order.ClientPhone}", normal)) {  MinimumHeight = 16, Border = 0 });

                table1.AddCell(new PdfPCell(new Phrase(" ", bold)) { MinimumHeight = 25, Colspan = 5,Border = 0});

                table1.AddCell(new PdfPCell(new Phrase("__________________________Глущенко І.М.", normal)) { MinimumHeight = 16, Colspan = 2, Border = 0});
                table1.AddCell(new PdfPCell(new Phrase(" ", normal)) {  MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("__________________________ПІ", normal)) { MinimumHeight = 16, Colspan = 2, Border = 0});

                table1.AddCell(new PdfPCell(new Phrase("дата", normal)) {  MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase(" ", normal)) {  MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase(" ", normal)) {  MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase("дата ", normal)) {  MinimumHeight = 16, Border = 0 });
                table1.AddCell(new PdfPCell(new Phrase(" ", normal)) {  MinimumHeight = 16, Border = 0 });

                document.Add(table1);

                document.NewPage();

                document.Add(new Paragraph("                                                                                                АКТ ПРИЙОМУ-ПЕРЕДАЧІ", head));

                //document.Add(new Paragraph($"                                                     до Договору найму № {orderNumber} від {order.Order.From.ToString("dd.MM.yyyy")}р.", normal));
                document.Add(new Paragraph($"м. Київ                                                                                                                                                                                                     " + DateTime.Now.Date.ToString("dd MMMM yyyy") + "р.", normal));
                document.Add(new Paragraph(" ", normal));
                document.Add(new Paragraph($"Фізична особа підприємець Глущенко Iнна Миколаївна з однієї сторони (далі в тексті – Наймодавець) та {order.Order.ClientLastName} {order.Order.ClientFirstName} (ПІ) з другої сторони (далі в тексті – Наймач), разом іменовані – Сторони, склали цей акт про передачу від Глущенко Iнни Миколаївни до {order.Order.ClientLastName} {order.Order.ClientFirstName} в рамках Договору № {orderNumber} від {DateTime.Now.Date.ToString("dd.MM.yyyy")}р.  наступного Майна", normal));
                document.Add(new Paragraph(" ", normal));

                float[] widths22 = new float[] { 10f, 30f,50f, 110f };

                PdfPTable table2 = new PdfPTable(widths22);

                table2.AddCell(new PdfPCell(new Phrase("№", normal)) { HorizontalAlignment = 1, MinimumHeight = 30, VerticalAlignment = Element.ALIGN_MIDDLE });
                table2.AddCell(new PdfPCell(new Phrase("Товарний код", normal)) { HorizontalAlignment = 1, MinimumHeight = 30, VerticalAlignment = Element.ALIGN_MIDDLE });
                table2.AddCell(new PdfPCell(new Phrase("Повна вартість майна", normal)) { HorizontalAlignment = 1, MinimumHeight = 30, VerticalAlignment = Element.ALIGN_MIDDLE });
                table2.AddCell(new PdfPCell(new Phrase("Примітки", normal)) { HorizontalAlignment = 1, MinimumHeight = 30, VerticalAlignment = Element.ALIGN_MIDDLE });

                int i = 1;
                foreach (var item in order.Order.OrderProducts)
                {
                    table2.AddCell(new PdfPCell(new Phrase((i++).ToString(), normal)) { HorizontalAlignment = 1, MinimumHeight = 25, VerticalAlignment = Element.ALIGN_MIDDLE });
                    table2.AddCell(new PdfPCell(new Phrase(item.Code, normal)) { HorizontalAlignment = 1, MinimumHeight = 25, VerticalAlignment = Element.ALIGN_MIDDLE });
                    table2.AddCell(new PdfPCell(new Phrase(item.FullPrice.ToString("f2"), normal)) { HorizontalAlignment = 1, MinimumHeight = 25, VerticalAlignment = Element.ALIGN_MIDDLE });
                    table2.AddCell(new PdfPCell(new Phrase("", normal)) { HorizontalAlignment = 1, MinimumHeight = 25, VerticalAlignment = Element.ALIGN_MIDDLE });
                }

                document.Add(table2);

                document.Add(new Paragraph(" ", normal));
                document.Add(new Paragraph(" ", normal));
                document.Add(new Paragraph(" ", normal));

                float[] widths1 = new float[] { 20f, 70f, 100, 20f, 70f };
                PdfPTable table3 = new PdfPTable(widths1);
                table3.WidthPercentage = 100;

                table3.AddCell(new PdfPCell(new Phrase("Наймодавець:", bold)) { MinimumHeight = 16, Colspan = 2, Border = 0 });
                table3.AddCell(new PdfPCell(new Phrase("   ", normal)) { MinimumHeight = 16, Border = 0 });
                table3.AddCell(new PdfPCell(new Phrase("Наймач: ", bold)) { MinimumHeight = 16, Colspan = 2, Border = 0 });

                table3.AddCell(new PdfPCell(new Phrase("", normal)) { MinimumHeight = 16, Border = 0 });
                table3.AddCell(new PdfPCell(new Phrase("", normal)) { MinimumHeight = 16, Border = 0 });
                table3.AddCell(new PdfPCell(new Phrase("   ", normal)) { MinimumHeight = 16, Border = 0 });
                table3.AddCell(new PdfPCell(new Phrase("", normal)) { MinimumHeight = 16, Border = 0 });
                table3.AddCell(new PdfPCell(new Phrase($"", normal)) { MinimumHeight = 16, Border = 0 });


                table3.AddCell(new PdfPCell(new Phrase(" ", bold)) { MinimumHeight = 25, Colspan = 5, Border = 0 });

                table3.AddCell(new PdfPCell(new Phrase("__________________________Глущенко І.М.", normal)) { MinimumHeight = 16, Colspan = 2, Border = 0 });
                table3.AddCell(new PdfPCell(new Phrase(" ", normal)) { MinimumHeight = 16, Border = 0 });
                table3.AddCell(new PdfPCell(new Phrase("__________________________ПІ", normal)) { MinimumHeight = 16, Colspan = 2, Border = 0 });

                table3.AddCell(new PdfPCell(new Phrase("дата", normal)) { MinimumHeight = 16, Border = 0 });
                table3.AddCell(new PdfPCell(new Phrase(" ", normal)) { MinimumHeight = 16, Border = 0 });
                table3.AddCell(new PdfPCell(new Phrase(" ", normal)) { MinimumHeight = 16, Border = 0 });
                table3.AddCell(new PdfPCell(new Phrase("дата ", normal)) { MinimumHeight = 16, Border = 0 });
                table3.AddCell(new PdfPCell(new Phrase(" ", normal)) { MinimumHeight = 16, Border = 0 });

                document.Add(table3);


                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));
                document.Add(new Paragraph("  ", normal));

                document.Add(new Paragraph("Передача Майна від Наймача до Наймодавця:", normal));
                
                PdfPTable table4 = new PdfPTable(widths1);
                table4.WidthPercentage = 100;
                document.Add(new Paragraph("  ", normal));
                table4.AddCell(new PdfPCell(new Phrase("Наймодавець:", bold)) { MinimumHeight = 16, Colspan = 2, Border = 0 });
                table4.AddCell(new PdfPCell(new Phrase("   ", normal)) { MinimumHeight = 16, Border = 0 });
                table4.AddCell(new PdfPCell(new Phrase("Наймач: ", bold)) { MinimumHeight = 16, Colspan = 2, Border = 0 });

                table4.AddCell(new PdfPCell(new Phrase("", normal)) { MinimumHeight = 16, Border = 0 });
                table4.AddCell(new PdfPCell(new Phrase("", normal)) { MinimumHeight = 16, Border = 0 });
                table4.AddCell(new PdfPCell(new Phrase("   ", normal)) { MinimumHeight = 16, Border = 0 });
                table4.AddCell(new PdfPCell(new Phrase("", normal)) { MinimumHeight = 16, Border = 0 });
                table4.AddCell(new PdfPCell(new Phrase($"", normal)) { MinimumHeight = 16, Border = 0 });


                table4.AddCell(new PdfPCell(new Phrase(" ", bold)) { MinimumHeight = 25, Colspan = 5, Border = 0 });

                table4.AddCell(new PdfPCell(new Phrase("__________________________Глущенко І.М.", normal)) { MinimumHeight = 16, Colspan = 2, Border = 0 });
                table4.AddCell(new PdfPCell(new Phrase(" ", normal)) { MinimumHeight = 16, Border = 0 });
                table4.AddCell(new PdfPCell(new Phrase("__________________________ПІ", normal)) { MinimumHeight = 16, Colspan = 2, Border = 0 });

                table4.AddCell(new PdfPCell(new Phrase("дата", normal)) { MinimumHeight = 16, Border = 0 });
                table4.AddCell(new PdfPCell(new Phrase(" ", normal)) { MinimumHeight = 16, Border = 0 });
                table4.AddCell(new PdfPCell(new Phrase(" ", normal)) { MinimumHeight = 16, Border = 0 });
                table4.AddCell(new PdfPCell(new Phrase("дата ", normal)) { MinimumHeight = 16, Border = 0 });
                table4.AddCell(new PdfPCell(new Phrase(" ", normal)) { MinimumHeight = 16, Border = 0 });

                document.Add(table4);

                document.Close();

                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;

                return workStream;
            }
            catch (Exception ex)
            {

            }

            return new MemoryStream();
        }
    }
}
