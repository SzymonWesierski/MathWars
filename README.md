# MathWars

MathWars to aplikacja webowa stworzona dla maturzystów, którzy chcą podszkolić się z matematyki w zakresie potrzebnym do zdania matury. Inspiracją dla projektu są strony takie jak CodeWars czy LeetCode, koncentrujące się na nauce programowania. Nasza aplikacja dostarcza użytkownikom zestawy zadań matematycznych na różnych poziomach trudności, umożliwiając interaktywne rozwiązywanie zadań i zdobywanie punktów za poprawne odpowiedzi.

## Członkowie Zespołu
- Michał Żołądek
- Jakub Osowski
- Szymon Węsierski

## Technologie i Narzędzia
- **Frontend:** ASP.NET z Razor Pages, Bootstrap, JavaScript (Redux, Material-UI)
- **Backend:** ASP.NET Core, Entity Framework, Identity Framework, SQL Server 2022
- **Wersjonowanie:** Git, GitHub
- **Testowanie:** NUnit, xUnit, Selenium

## Funkcjonalności Aplikacji
- Rejestracja i Logowanie
- Różnorodne poziomy trudności
- System punktacji
- Ranking użytkowników
- Interaktywne rozwiązywanie zadań
- Materiały edukacyjne

## Milestones (Etapy Projektu)
1. **Planowanie i projektowanie interfejsu użytkownika (UI/UX):**
   - Określenie struktury i wyglądu stron Razor Pages.
   - Projektowanie interakcji użytkownika i układu elementów w technologii Razor Pages.

2. **Rozwój Backendu:**
   - Implementacja logicznych funkcji za pomocą stron Razor Pages.
   - Wykorzystanie Entity Framework do integracji z bazą danych SQL Server 2022.
   - Konfiguracja Identity Framework do zarządzania tożsamościami użytkowników.

3. **Rozwój Frontendu:**
   - Stylizacja interfejsu użytkownika przy użyciu elementów Bootstrap.
   - Dodanie interaktywności przy użyciu JavaScript na stronach Razor Pages.
   - Ewentualne użycie bibliotek, takich jak Redux, do zarządzania stanem aplikacji.

4. **Testowanie funkcjonalności:**
   - Przeprowadzenie testów jednostkowych dla logiki biznesowej implementowanej w stronach Razor Pages.
   - Testowanie interakcji użytkownika na stronach za pomocą testów UI, możliwe z wykorzystaniem narzędzi takich jak Selenium.

5. **Wdrażanie i uruchamianie aplikacji w środowisku produkcyjnym:**
   - Przygotowanie aplikacji do środowiska produkcyjnego w oparciu o technologię Razor Pages.
   - Wdrożenie aplikacji na serwerze produkcyjnym.
   - Konfiguracja i monitorowanie aplikacji w środowisku produkcyjnym.

## Uwagi Dotyczące Bezpieczeństwa i Autoryzacji
- Domyślny administrator (admin) jest zdefiniowany w pliku `appsettings.json`. W przypadku usunięcia, mechanizm startowy utworzy nowego administratora.
- Podstawowe role są także tworzone automatycznie przy starcie aplikacji.
- Role decydują o dostępie do poszczególnych funkcji. Administrator ma pełen dostęp, użytkownik widzi ograniczony zestaw opcji, a menedżer zadań ma dostęp do CRUD dla zadań i kategorii zadań.
- W Leaderboard filtrowani są tylko użytkownicy z rolą user.

## Instrukcje dla Deweloperów
1. Sklonuj repozytorium: `git clone https://github.com/SzymonWesierski/MathWars.git`
2. W pliku appstring zmienaimy nazwę serwera
3. W Menedżerze pakietów wpisujemy update-database
4. Uruchom aplikację lokalnie
5. Przeglądaj aplikację w przeglądarce pod adresem `https://localhost:7069/`

---
*Ostatnia aktualizacja: [Data aktualizacji]*

