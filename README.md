# Projekty z Grafiki Komputerowej

Repozytorium zawiera cztery projekty związane z grafiką komputerową. Każdy projekt koncentruje się na różnych aspektach grafiki 2D i 3D, w tym edycji wielokątów, renderowaniu powierzchni Beziera, animacji obrazów oraz tworzeniu sceny 3D z wykorzystaniem programowalnego potoku renderowania.

## 1. Edytor Wielokątów
Interaktywny edytor umożliwiający manipulację wielokątami oraz segmentami Beziera. Kluczowe funkcje:
- Dodawanie, edytowanie i usuwanie wielokątów
- Przesuwanie wierzchołków i punktów kontrolnych segmentów Beziera
- Możliwość dodawania ograniczeń (relacji) dla krawędzi:
  - Krawędź pozioma, pionowa
  - Zadana długość krawędzi
- Konwersja krawędzi na segmenty Beziera 3-go stopnia z różnymi klasami ciągłości (G0, G1, C1)
- Algorytmy rysowania:
  - Odcinki: algorytm biblioteczny oraz własna implementacja (Bresenhama)
  - Segmenty Beziera: iteracyjny algorytm wyznaczania punktów po konwersji do bazy potęgowej
- Predefiniowana scena z ograniczeniami i co najmniej jednym segmentem Beziera

## 2. Powierzchnia Beziera
Program renderujący powierzchnię Beziera 3-go stopnia na podstawie punktów kontrolnych wczytywanych z pliku. Funkcje:
- Obrót powierzchni wokół osi Z i X (kąty alfa i beta regulowane suwakami)
- Triangulacja powierzchni i interpolacja trójkątami
- Rzut prostokątny powierzchni na płaszczyznę XY
- Możliwość wyboru trybu renderowania: siatka / wypełnienie trójkątów
- Oświetlenie z modelem Lamberta i składową zwierciadlaną
- Możliwość animacji ruchu źródła światła po spirali
- Wsparcie dla tekstur i map wektorów normalnych

## 3. Ruch Obrazka
- Animacja obrazu poruszającego się wzdłuż krzywej
- Obraz obraca się stycznie do krzywej
- Możliwość zatrzymania ruchu i przełączenia na obrót wokół środka
- Obsługa naiwnego obrotu oraz filtracji
- Interaktywne przesuwanie wierzchołków krzywej w trakcie animacji

## 4. Scena 3D
Projekt wykorzystujący API graficzne (np. OpenGL, DirectX, WebGL) do renderowania interaktywnej sceny 3D.

### Główne cechy:
- Programowalny potok renderowania (shadery)
- Obiekty w scenie: 
  - Jeden poruszający się obiekt (z ruchem i obrotami)
  - Kilka stałych obiektów (w tym co najmniej jeden gładki)
- Przełączalne kamery:
  - Nieruchoma obserwująca scenę
  - Nieruchoma śledząca obiekt poruszający się
  - Kamera związana z obiektem ruchomym (FPP/TPP)
- Oświetlenie:
  - 3 źródła światła (w tym reflektor na obiekcie ruchomym)
  - Ręczna zmiana kierunku świecenia reflektora
  - Stałe źródło światła (punktowe lub reflektor)
- Efekty:
  - Rzutowanie perspektywiczne
  - Model cieniowania Phonga (interpolacja normalnych)
  - Efekt mgły
  - Przełączanie między dniem a nocą
  - Zanikanie światła wraz z odległością
