using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class Program
    {
        static void Main(string[] args)
        {
            var graph = new Agirlikli_Yonsuz_Graf();

            graph.Kenar_Ekle("Görükle", "19 Mayıs", 120);
            graph.Kenar_Ekle("Görükle", "Özlüce", 140);
            graph.Kenar_Ekle("19 Mayıs", "Özlüce", 60);
            graph.Kenar_Ekle("19 Mayıs", "Yüzüncüyıl", 40);
            graph.Kenar_Ekle("Özlüce", "Yüzüncüyıl", 90);
            graph.Kenar_Ekle("Özlüce", "1.Acil Birimi", 30);
            graph.Kenar_Ekle("Özlüce", "29 Ekim", 100);
            graph.Kenar_Ekle("Özlüce", "Altınşehir", 100);
            graph.Kenar_Ekle("Yüzüncüyıl", "1.Acil Birimi", 10);
            graph.Kenar_Ekle("Yüzüncüyıl", "29 Ekim", 30);
            graph.Kenar_Ekle("Üçevler", "2.Acil Birimi", 40);
            graph.Kenar_Ekle("Üçevler", "Ataevler", 70);
            graph.Kenar_Ekle("Üçevler", "Beşevler", 60);
            graph.Kenar_Ekle("Üçevler", "Demirci", 40);
            graph.Kenar_Ekle("29 Ekim", "Altınşehir", 70);
            graph.Kenar_Ekle("29 Ekim", "1.Acil Birimi", 15);
            graph.Kenar_Ekle("29 Ekim", "Üçevler", 100);
            graph.Kenar_Ekle("Altınşehir", "Üçevler", 80);
            graph.Kenar_Ekle("Altınşehir", "Ataevler", 110);
            graph.Kenar_Ekle("Altınşehir", "2.Acil Birimi", 50);
            graph.Kenar_Ekle("Ataevler", "2.Acil Birimi", 30);
            graph.Kenar_Ekle("Ataevler", "Beşevler", 40);

            //graph.Graf_Yazdir();
            Console.Write("Başlangıç noktasını girin: ");
            string baslangic = Console.ReadLine().ToLower();

            var enKisaYollar = graph.Dijkstra(baslangic);

            string hedef1 = "1.acil birimi";
            string hedef2 = "2.acil birimi";

            int mesafe1 = enKisaYollar.ContainsKey(hedef1) ? enKisaYollar[hedef1] : int.MaxValue;
            int mesafe2 = enKisaYollar.ContainsKey(hedef2) ? enKisaYollar[hedef2] : int.MaxValue;

            Console.WriteLine("\n{0} noktasından en kısa yollar:", baslangic);

            if (!(mesafe1 == int.MaxValue))
                Console.WriteLine("{0} : {1}", hedef1, mesafe1);
            if (!(mesafe2 == int.MaxValue))
                Console.WriteLine("{0} : {1}", hedef2, mesafe2);

            if (mesafe1 == int.MaxValue && mesafe2 == int.MaxValue)
            {
                Console.WriteLine("\nHer iki acil birimine de ulaşılamıyor.");
            }
            else if (mesafe1 < mesafe2)
            {
                Console.WriteLine("\n{0} daha yakındır.", hedef1);
            }
            else if (mesafe2 < mesafe1)
            {
                Console.WriteLine("\n{0} daha yakındır.", hedef2);
            }
            else
            {
                Console.WriteLine("\nHer iki acil birime eşit uzaklıktasınız.");
            }
        }
    }

    public class Agirlikli_Yonsuz_Graf
    {
        public Dictionary<string, List<(string komsu, int maliyet)>> komsuluk_listesi;


        public Agirlikli_Yonsuz_Graf()
        {
            komsuluk_listesi = new Dictionary<string, List<(string komsu, int maliyet)>>();
        }

        public void Dugum_Ekle(string isim)
        {
            if (!komsuluk_listesi.ContainsKey(isim))
            {
                komsuluk_listesi[isim] = new List<(string, int)>();
            }
        }

        public void Kenar_Ekle(string baslangic_dugum, string son_dugum, int maliyet)
        {
            baslangic_dugum = baslangic_dugum.ToLower();
            son_dugum = son_dugum.ToLower();
            
            Dugum_Ekle(baslangic_dugum);
            Dugum_Ekle(son_dugum);

            komsuluk_listesi[baslangic_dugum].Add((son_dugum, maliyet));
            komsuluk_listesi[son_dugum].Add((baslangic_dugum, maliyet));
        }

        public void Graf_Yazdir()
        {
            foreach (var dugum in komsuluk_listesi)
            {
                Console.Write(dugum.Key + " -> ");
                foreach (var (komsu, maliyet) in dugum.Value)
                {
                    Console.Write("{0} {1} m, ", komsu, maliyet);
                }
                Console.WriteLine();
            }
        }
        public Dictionary<string, int> Dijkstra(string baslangic)
        {
            baslangic = baslangic.ToLower();

            var mesafeler = new Dictionary<string, int>();
            var onceki = new Dictionary<string, string>();
            var kuyruk = new PriorityQueue<string, int>();

            // Başlangıçta tüm mesafeler sonsuz, başlangıç noktasına 0
            foreach (var dugum in komsuluk_listesi.Keys)
            {
                mesafeler[dugum] = int.MaxValue;
            }

            if (!komsuluk_listesi.ContainsKey(baslangic))
            {
                Console.WriteLine("Hatalı başlangıç düğümü girdiniz.");
                return mesafeler;
            }

            mesafeler[baslangic] = 0;
            kuyruk.Enqueue(baslangic, 0);

            while (kuyruk.Count > 0)
            {
                kuyruk.TryDequeue(out string mevcut, out int mevcutMesafe);

                foreach (var (komsu, maliyet) in komsuluk_listesi[mevcut])
                {
                    int yeniMesafe = mesafeler[mevcut] + maliyet;
                    if (yeniMesafe < mesafeler[komsu])
                    {
                        mesafeler[komsu] = yeniMesafe;
                        onceki[komsu] = mevcut;
                        kuyruk.Enqueue(komsu, yeniMesafe);
                    }
                }
            }
            return mesafeler;
        }
    }
}