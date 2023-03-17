HL_horgaszas_script
===========

**Todok**:
1. Mta teljes képernyős módban
2. Tabolás helyett fókuszálni az ablakot
3. A csali a hotbarra rakva való használata
4. Bedobási algoritmus kiszámítása
5. Meghatározni, hogy mennyi ideig várjon, ha rájön hogy kapása van
6. Code cleanup
7. A kurzor pozícióját timer-t használva oldom meg
8. Kinézet cleanup (több page)
9. A horgászbotot is előveszi
10. Eladja a halakat amiket fogott (mikor beálltunk a helyünkre, akkor megadjuk, hogy melyik kordin van az npc)
 -Nyom egy m betűt ráviszi az egeret és jobbklikk
11. Egy gyorsgombbal le lehessen állítani a programot

Problémák és a megoldáshoz linkek:
--runtime mappa létrehozásának megoldása
[stackoverflow question](https://stackoverflow.com/questions/67920055/superfluous-runtimes-folder-created-in-output-directory-for-net-5-project)
--Thread.Sleep() alternatíva, amely pontosabb és jobb is, mert nem blokkolj a threadet, illetve nem szedi ki a cachet
[link](https://stackoverflow.com/questions/5424667/alternatives-to-thread-sleep)


random felesleges ötletek:
-Olvassa a chatet, és keresi, hogy leírták-e az ic nevünket, ha leírták, akkor vagy elküldi email/messenger-en, vagy elküldi az üzit a chatgpt-nek és amit kapott választ azt leírja ic chatre
-Szintén olvassa a chatet, és figyeli, hogyha írt egy admin a chatre akkor küld egy üzenetet telóra
-Ha errorba ütközik, akkor kilép a szerverről
-console window always on top és logol mint a minecraft szerók 😎
