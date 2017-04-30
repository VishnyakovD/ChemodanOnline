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
                var head = new Font(sylfaen, 10f, Font.NORMAL, BaseColor.BLACK);
                var normal = new Font(sylfaen, 10f, Font.NORMAL, BaseColor.BLACK);
                var underline = new Font(sylfaen, 10f, Font.UNDERLINE, BaseColor.BLACK);

                document.Open();

                var orderNumber = ((order.Order.OrderPrefix > 0)
                    ? $"{order.Order.OrderNumber}-{order.Order.OrderPrefix}"
                    : $"{order.Order.OrderNumber}");

                document.Add(new Paragraph($"                                                                                  Договір найму №{orderNumber}", head) );

                document.Add(new Paragraph(" ", normal));
                document.Add(new Paragraph($"м. Київ                                                                                                                                                    дата "+ DateTime.Now.Date.ToString("dd.MM.yyyy")+"р.",normal));
                document.Add(new Paragraph(" ", normal));
                document.Add(new Paragraph(" ", normal));
                document.Add(new Paragraph(" ", normal));
                document.Add(new Paragraph($"Фізична особа підприємець Глущенко Iнна Миколаївна з однієї сторони (далі в тексті – Наймодавець) та {order.Order.ClientLastName} {order.Order.ClientFirstName} (ПІ) з другої сторони (далі в тексті – Наймач), разом іменовані – Сторони, уклали цей договір про наступне:", normal));
                document.Add(new Paragraph(" ", normal));
                document.Add(new Paragraph("1.  Даний Договір є договором приєднання у контексті статті 634 Цивільного кодексу України.", normal));
                document.Add(new Paragraph("2.  У цьому Договорі під терміном “Сайт” розуміється розташований в мережі Інтернет сайт за адресою https://chemodan.online.", normal));
                document.Add(new Paragraph("3.  Терміни та поняття, значення яких не розкрито в цьому Договорі, розуміються Сторонами відповідно чинного законодавства України.", normal));
                document.Add(new Paragraph("4.  Наймодавець зобов'язується надавати Наймачу за плату в  тимчасове користування валізи/дорожні сумки та/або чохла на валізу/дорожню сумку (надалі за текстом – \"Майно\") у порядку та на умовах встановлених в цьому Договорі.", normal));
                document.Add(new Paragraph("5.  Прийняття умов Договору та його укладення є здійснення  Наймачем дій, визначених п. 8 цього Договору.", normal));
                document.Add(new Paragraph("6.  Строк прийняття пропозиції Наймачем від Наймодавця є необмеженим. Прийняття умов Договору означає повну і беззаперечну згоду Наймача зі всіма умовами Договору без будь-яких виключень і/або обмежень і рівносильний укладенню письмового Договору відповідно до положень законодавства України. (ч. 2 ст. 642 ЦК України).", normal));
                document.Add(new Paragraph("7.  Місцем укладення  Договору є місцезнаходження  Наймодавця. Місцем виконання Договору є місце передачі в найм Майна. ", normal));
                document.Add(new Paragraph("8.  Для укладення Договору в простій письмовій формі Наймачу необхідно підписати Договір зміст якого викладеній на папері і який вважається укладеним з моменту його підписання обома Сторонами. ", normal));
                document.Add(new Paragraph($"9.  Майно передається Наймодавцем Наймачу не пізніше {order.Order.CreateDate.ToString("dd.MM.yyyy")}, а Наймач повинен прийняти таке Майно. Претензії щодо невідповідності Майна його опису на сайті та його придатності для користування приймаються виключно в день отримання Наймачем Майна (у випадку якщо воно отримане шляхом доставки кур’єром), а у випадку отримання Майна у пункті прийому-видачі за адресою: пр. Перемоги, 67, корпус “I”, офіс 213  - тільки до моменту підписання акту прийому-передачі Майна. Разом із Майном Наймачу передається квитанція.", normal)); 
                document.Add(new Paragraph("10.  Документом, який підтверджує передачу або повернення Майна із зазначенням стану Майна є акт прийому - передачі. ", normal));
                document.Add(new Paragraph("11.  Майно, надане за даним Договором повинно використовуватися Наймачем виключно за його цільовим призначенням. Враховуючи особливості використання Майна, Наймач зобов’язується у процесі використання здійснювати всі можливі дії для його збереження у стані в якому Майно було ним отримано від Наймодавця.", normal));
                document.Add(new Paragraph("12.  Наймач зобов`язаний повернути Майно Наймодавцю не пізніше останнього дня користування Майном згідно строків встановлених у п. 14 Договору та у стані, який є не гірший (з урахуванням нормального зносу) ніж на момент отримання Майна Наймачем. ", normal));
                document.Add(new Paragraph("13.  З метою забезпечення виконання Наймачем зобов`язань передбаченого п. 11 та п.12 цього Договору Наймач повинен сплатити Наймодавцю грошову заставу у розмірі повної вартості Майна. Наймач не має право користуватися сплаченою грошовою заставою до моменту повернення Майна. Сторони погоджуються, що у випадку порушень Наймачем зобов’язань встановлених в п. 11 та п. 12 Договору, Наймодавець  без будь-яких додаткових погоджень із Наймачем або звернень до суду чи інших осіб набуває право власності на заставу у розмірі який встановлюється із врахуванням п. 14, п. 15, та п. 16 цього Договору.", normal));
                document.Add(new Paragraph("Грошова застава повертається Наймодавцем Наймачу за вирахуванням тієї частини, на яку Наймодавець набув право власності. В результаті порушень Наймачем зобов’язань передбачених п. 11 та п.12 Договору після моменту повернення Майна (якщо таке повернення було здійснено Наймачем в пункт прийому-видачі Майна) Наймодавцю з грошової застави окрім вирахування тієї частини, на яку Наймодавець набув право власності також вираховується штраф у розмірі вартості найму Майна за кожну добу користування Майном понад строки визначені в Договорі та/або відшкодування за пошкодження Майна в розмірі передбачених п.15.", normal));
                decimal days = (decimal)(Math.Round((order.Order.To - order.Order.From).TotalDays));
                document.Add(new Paragraph($"14.  Строк користування Майном становить {days} календарних днів починаючи з {order.Order.From.ToString("dd.MM.yyyy")} по {order.Order.To.ToString("dd.MM.yyyy")} включно.", normal));
                document.Add(new Paragraph("    14.1.  Майно, що передається Наймачу:", normal));
                document.Add(new Paragraph("", normal));//тут вставить таблицу
                document.Add(new Paragraph($"    14.2.  Загальна вартість прокату Майна  за період передбачений в п. 14 становить  {(order.Order.OrderProducts.Sum(item => item.PriceDay) * days).ToString("F2")} грн.;", normal));
                document.Add(new Paragraph("", normal));
                document.Add(new Paragraph("", normal));
                document.Add(new Paragraph("", normal));
                document.Add(new Paragraph("", normal));
                document.Add(new Paragraph("", normal));
                document.Add(new Paragraph("", normal));






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
