# SIEM-system

Programski jezik:
C#

Razvojna okolina:
Microsoft Visual Studio Ultimate 2013

Frameworks:
.NET 4.5, LINQ, SOAP, NLog 4.3.7

Cilj projekta je napraviti prototip SIEM sustava korištenjem NLog 4.3.7 frameworka za logiranje unutar postojeće "Skype for bussines" aplikacije. Osnovna funkcionalnost sustava za upravljanje informacijama i događajima je prikaz, pohrana i dinamički ispis log poruka. Upotrebom klase Socket i TCP protokola razvijeni su server i klijent koji simuliraju sustav za asinkronu i višedretvenu komunikaciju. Sustav za logiranje mora biti u stanju pratiti sve važne događaje i slati feedback u više sourceva. Najpopularnije značajke sustava za komunikaciju su razmjena poruka i kolekcija podataka, a osim toga, logging sustav mora ispravno raditi i prilikom delay-ja, connect-a, disconnect-a i lock-a. Kolekcije koje se šalju putem Socket-a potrebno je serijalizirati na jednoj i deserijalizirati na drugoj strani prilikom čega je korišten SOAP protokol, te klase MemoryStream i XElement za izradu XML stabala. Sustav podržava značajke poput automatskog generiranja naziva datoteke i automatskog zapisa u drugu datoteku ukoliko on premaši određenu vrijednost, dok se kompletna konfiguracija loggera vrši putem datoteke NLog.config.
