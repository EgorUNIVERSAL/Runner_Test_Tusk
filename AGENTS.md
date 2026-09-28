# Локальный контекст проекта Runner_test_tusk

> Снимок состояния проекта на момент анализа. Этот файл предназначен для следующих разработчиков/AI-агентов: перед изменениями сверяйтесь с требованиями и известными проблемами ниже.

## Обязательное правило ведения контекста

- Всю важную информацию, обнаруженную или созданную в ходе дальнейшей работы, необходимо сразу фиксировать в этом `AGENTS.md`.
- Важной считается информация, влияющая на требования, архитектуру, настройки, зависимости, запуск, сборку, тестирование, интеграции, принятые решения, ограничения, известные ошибки и текущее состояние реализации.
- После существенных изменений обновлять устаревшие разделы, а не только добавлять новые заметки; контекст должен отражать фактическое состояние проекта.
- Не записывать секреты: access tokens, пароли, API keys, приватные credentials и другие чувствительные значения. Вместо них фиксировать только способ настройки и имена необходимых переменных окружения.
- Не засорять контекст временными логами и малозначимыми деталями, которые не помогут продолжить работу в новой сессии.

## 1. Назначение и требования

Это тестовое задание: воспроизвести первый уровень мобильного раннера **Rich Inc.** максимально близко к референсу, включая механику, управление, анимации, UI и эффекты.

Исходный документ: `Тестовое задание Programmer.docx`.

Ссылки из документа:

- референс в Google Play: https://play.google.com/store/apps/details?id=com.ohmgames.richtopoor
- видео ожидаемого результата: https://drive.google.com/file/d/1Vd_fSF3d7_NdNNMYrIn2T7u78GZeGwrl/view?usp=sharing
- необходимые ассеты: https://drive.google.com/drive/folders/1l9OVXK81nPCKHjT59URhf8ZcPyd9A3l0?usp=sharing
- предоставленный менеджер уровней: https://drive.google.com/file/d/1u7xLOmAnrZ65ECHbE-90GT6ffS7syHGP/view?usp=sharing

Требуется реализовать:

- gameplay-сцену первого уровня;
- камеру и освещение;
- игровой UI без магазина и мета-игры;
- туториал;
- управление, в точности повторяющее референс;
- game loop: стартовый UI → старт → уровень → победа/поражение → следующий уровень;
- препятствия;
- пикапы: деньги и алкоголь;
- области с флагами;
- ворота;
- финишную область;
- шкалу, меняющуюся от пикапов;
- изменение модели персонажа в зависимости от шкалы;
- анимации, эффекты и общий polish.

Формат сдачи по документу: минутное gameplay-видео на YouTube (первый уровень и переход к следующему; первый уровень разрешено дублировать) и ссылка на GitHub. Оценивается также качество кода. Указанный срок — одни сутки.

## 2. Технологии и настройки

- Unity `6000.2.6f2` (Unity 6.2), см. `ProjectSettings/ProjectVersion.txt`.
- C#, основная пользовательская сборка — `Assembly-CSharp`.
- Universal Render Pipeline `17.2.0`.
- Input System `1.14.2`; в Player Settings стоит `activeInputHandler: 2` (Both), поэтому текущий legacy `UnityEngine.Input` работает наряду с новой Input System.
- Также подключены uGUI `2.0.0`, ProBuilder `6.0.9`, AI Navigation `2.0.14`, Visual Scripting `1.9.7`, Timeline и Unity Test Framework.
- Цветовое пространство Linear (`m_ActiveColorSpace: 1`).
- Product name: `Runner_test_tusk`, версия `0.1.0`, компания пока `DefaultCompany`.
- Default resolution: 1024×768; Android fullscreen включён, архитектура ARM64 (`AndroidTargetArchitectures: 2`).
- Пользовательские слои: `Ground` (3), `Water` (4), `UI` (5), `Player` (6). Пользовательский tag: `Ground`.

## 3. Важная структура

- `Assets/Project/` — собственная часть проекта:
  - `Scenes/Level_1.unity` — собранный черновик трассы;
  - `Scenes/New Scene.unity` — практически пустая сцена;
  - `Scripts/` — 22 runtime/editor C#-скрипта, включая gameplay, UI, debug helpers и предоставленный LevelManager;
  - `Arts/Prefabs/` — Bottle, Money, флаги, player и другие FBX/prefab;
  - `Animations/`, `Materials/`, `Sounds/` — локальные игровые ресурсы.
- `Assets/Assets/Assets/` — большой импортированный набор референсных ресурсов (около 1539 файлов, ~206 MiB без `.meta`): меши, текстуры, sprites, материалы, шейдеры, звуки, шрифты. Вложенность `Assets/Assets/Assets` необычная, но менять пути без необходимости нельзя: это сломает ссылки GUID/ассетов.
- `Assets/Fantasy Skybox FREE/` — сторонний skybox-пак (~121 файл, ~133 MiB) с двумя demo-сценами.
- `Assets/Scenes/SampleScene.unity` — стандартная/тестовая сцена; сохранена в проекте, но исключена из Build Settings. Единственная стартовая build-сцена — `Assets/Project/Scenes/Level_1.unity`.
- `Assets/Settings/` — URP assets/renderers и volume profiles.
- `Packages/manifest.json` — зависимости.
- `ProjectSettings/` — настройки Unity.
- `Library/`, `Temp/`, `Logs/`, `obj/`, `.vs/`, `UserSettings/` — локальные/generated данные; не редактировать как исходники и не добавлять в VCS.

Каталог подготовлен как Git-репозиторий для публикации в публичном GitHub-репозитории `EgorUNIVERSAL/Runner_Test_Tusk`. Добавлен стандартный Unity `.gitignore`: generated-каталоги, локальные инструменты, IDE-файлы, APK и служебные каталоги Android/IL2CPP-сборок не публикуются. Файлы `.csproj` и `.sln` генерируются Unity и также исключены.

## 4. Имеющийся код

Все игровые классы находятся в namespace `ButchersGames`.

- `GameManager` реализует состояния `Start`, `Playing`, `Win`, `Lose`, запускает/останавливает игрока и перезагружает активную сцену для restart/next.
- `PlayerController` двигает `CharacterController` вдоль локального `forward`, хранит lateral offset вдоль локального `right`, применяет гравитацию и выполняет плавные повороты coroutine `StartTurn`.
- `SwipeInput` обрабатывает mouse/touch drag через legacy `UnityEngine.Input`; A/D и стрелки доступны в Editor. Player reference в Level_1 назначен.
- `TurnZone` вызывает поворот игрока; в Level_1 установлены четыре trigger-зоны.
- `CameraFollow` рассчитывает позицию через distance/pitch/side offset и плавно смотрит в точку впереди персонажа.
- `PlayerStats` хранит значение шкалы и монеты; coin даёт `+1`, bottle даёт `-10`, gates/flags выполняют арифметические операции.
- `Coin`, `Bottle`, `Gate`, `FlagZone`, `Obstacle`, `FinishZone` реализуют trigger-механику соответствующих объектов.
- `PlayerAnimator` управляет float-параметром Animator `Speed` и скоростью Animator.
- `UIManager` переключает start/game/win/lose screens и обновляет HUD; `PlayerHud` обновляет world-space slider; `TapToStart` запускает игру.
- `TestStats` остаётся прикреплённым к production-player и пишет debug-логи/обрабатывает клавиши 1–6. `TestStart` есть на отдельном `Player.prefab`, но не используется в Level_1.

### Level Manager

Путь: `Assets/Project/Scripts/Level Manager/Level Manager/`.

- `LevelManager.cs` — singleton-подобный менеджер, хранит прогресс в `PlayerPrefs`, инстанцирует prefab уровня из `LevelsList`, умеет restart/next/prev/select.
- `Level.cs` — marker-компонент с `playerSpawnPoint`; точка пока нигде не используется runtime-кодом.
- `LevelsList.cs` — `ScriptableObject` со списком `Level` и флагом случайного выбора.
- `Editor/LevelManagerEditor.cs` — custom inspector: editor mode, выбор уровня, удаление сохранений.

В проекте не найден созданный asset `LevelsList`, prefab с компонентом `Level` или сцена с подключённым `LevelManager`; система уровней фактически не интегрирована.

## 5. Фактическое состояние сцен

### Build Settings

В `ProjectSettings/EditorBuildSettings.asset` включена единственная стартовая сцена:

- `Assets/Project/Scenes/Level_1.unity` (build index 0).

`SampleScene.unity` исключена из списка, поэтому Player build должен запускать gameplay-сцену.

### `Assets/Scenes/SampleScene.unity`

Небольшая сцена (~42 KiB): Main Camera, Directional Light, Global Volume и импортированная модель `AfricanGirl_Descent`; игрового цикла и собственных gameplay-компонентов нет.

### `Assets/Project/Scenes/Level_1.unity`

Gameplay-сцена существенно дополнена:

- player root имеет tag `Player`, layer `Player`, `CharacterController`, `PlayerController`, `PlayerStats`; визуальный `player.fbx` является дочерним prefab-instance;
- `GameManager`, `TapToStart`, `SwipeInput` и все их ключевые ссылки назначены;
- камера использует `CameraFollow`, target назначен; есть Directional Light;
- 5 прямых ground-сегментов, 4 поворота и 4 `TurnZone` trigger;
- 36 `Money.prefab` и 12 `Bottle.prefab`: trigger collider и gameplay script есть, все 48 scene-instances получают общий `AudioSource` камеры и pickup clip;
- 3 области `FlagZone`, каждая содержит левый/правый флаг, trigger, звук и множитель ×2;
- 2 ворот: green ×2 и red ÷2;
- 4 trigger-препятствия с `Obstacle(killOnHit=true)`;
- trigger-финиш с `FinishZone`;
- два Canvas: screen-space start/game/win/lose UI с двумя кнопками и world-space slider над игроком;
- AnimatorController персонажа содержит Idle/Run и параметр `Speed`.

Сериализованные ссылки основных gameplay-компонентов назначены; missing-script markers в project scenes/prefabs не найдены. Level_1 назначен единственной стартовой build-сценой.

## 6. Критические проблемы и несоответствия требованиям

Повторный статический аудит показывает, что core loop и большая часть объектов первого уровня уже реализованы, но сдавать проект пока рано.

### Ошибки, реально ломающие или заметно нарушающие функционал

1. **Смена модели от шкалы полностью отсутствует.** `OnValueChanged` слушают только два HUD-компонента и debug `TestStats`; ни один код не переключает meshes/models/material tiers. Это прямое невыполнение обязательного требования.
2. **Next Level не использует LevelManager и не хранит прогресс.** `GameManager.NextLevel()` просто перезагружает активную сцену. Это визуально может считаться разрешённым дублированием первого уровня, но номера/прогресса/реального переключения нет, а предоставленный LevelManager не интегрирован.
3. **Предоставленный LevelManager небезопасен для Player build:** runtime-файл содержит безусловный `using UnityEditor`; кроме того, setter `CurrentLevel` вызывает `GetInt`, а не `SetInt`. Пока менеджер не используется, основной Level_1 от этого не падает, но его подключение/Player build создаст проблему.

Исправлено после аудита:

- Build Settings теперь содержит только `Level_1.unity` с build index 0; SampleScene удалена из build list.
- `FlagZone` запускает реальные Animator states `Left_flag_UP` и `Right_flag_up` с normalized time 0, поэтому обе анимации рестартуют синхронно при входе игрока.

### Остальные несоответствия и риски

- Управление нельзя признать «1 в 1» без сравнения на устройстве с референсом. Sensitivity основана на необработанной pixel delta (`0.012`) и не нормализуется по DPI/разрешению; поведение будет различаться между экранами.
- `TapToStart` отвергает нажатия поверх UI, при этом оба TMP-элемента StartScreen имеют `raycastTarget=true`. Нажатие непосредственно по надписям `Test_tusk`/`Slide to start!` не запускает игру; тап вне текста запускает. Это заметный UX-дефект туториала.
- В `Lose screen` текст кнопки `Again!` сделан отдельным TMP-sibling после объекта `Button`, растянут anchors 0..1 и имеет `raycastTarget=true`; он перекрывает кнопку и перехватывает UI-клики. Нужно сделать текст дочерним объектом Button или отключить Raycast Target у TMP.
- `TestStats` оставлен на production-player: генерирует debug logs и добавляет тестовые keyboard-команды. `TestStart` на `Player.prefab` автоматически запускает бег в обход `GameManager`, если этот prefab будет использован позднее.
- Есть шкала и арифметика pickups/gates/flags, но диапазон Level_1 равен 1…999, coin даёт только +1; визуальная динамика slider может быть почти незаметной и пороги состояний персонажа отсутствуют.
- Нет отдельной fail-логики при достижении минимального значения шкалы; поражение вызывают только четыре obstacle triggers. Требуемое поведение нужно сверить с референсом.
- Polish частичный: pickup/flag sounds и Idle/Run существуют, но flag animation сломана; отдельных VFX, feedback изменения модели и полноценного финишного celebration не выявлено.
- Source-файлы с русскими `[Header]`, `[Tooltip]` и комментариями сохранены с повреждённой/несогласованной кодировкой, поэтому Inspector показывает mojibake. Runtime это обычно не ломает, но снижает качество.
- Автоматических EditMode/PlayMode тестов и test asmdef нет.
- Стартовый UI — только текстовая подсказка; соответствие референсу, safe area и мобильные aspect ratios не проверены.

### Соответствие требованиям на текущий момент

| Требование | Статус | Наблюдение |
|---|---|---|
| Gameplay-сцена | Реализовано базово | Level_1 собран и назначен build index 0 |
| Камера/освещение | Реализовано базово | Follow camera + Directional Light |
| Игровой UI | Реализовано базово | Start/game/win/lose, HUD, кнопки restart/next |
| Туториал | Частично | `Slide to start!`, есть UI-raycast дефект |
| Управление как в референсе | Не подтверждено | drag и повороты есть, device/reference test отсутствует |
| Start/win/lose/restart | Реализовано базово | State machine подключена |
| Следующий уровень | Частично | reload той же сцены, без прогресса/LevelManager |
| Препятствия | Реализовано базово | 4 kill triggers |
| Монеты/алкоголь | Реализовано базово | 36/12, stats и SFX подключены |
| Области с флагами | Реализовано базово | stats/SFX есть, Animator state names исправлены |
| Ворота | Реализовано базово | ×2 и ÷2 |
| Финиш | Реализовано базово | trigger вызывает Win |
| Шкала от pickups | Реализовано базово | два slider, диапазон требует настройки |
| Смена модели от шкалы | Не реализовано | отсутствуют subscribers/model tier logic |
| Animation/effects/polish | Частично | Idle/Run и SFX есть; VFX/flag/final polish недостаточны |
| Максимально 1:1 | Не подтверждено | нужен side-by-side gameplay review |

### Ошибки/риски в предоставленном `LevelManager`

- Setter `CurrentLevel` вызывает `PlayerPrefs.GetInt(...)`, а не `SetInt(...)`; значение не сохраняется.
- Индексация смешивает 0-based и 1-based значения и может пропускать первый уровень/выбирать неверный индекс.
- В non-editor ветке `GetCorrectedIndex` фактически игнорирует переданный `levelIndex` в пользу `CurrentLevel`.
- Singleton назначается в конструкторе `MonoBehaviour`; корректнее делать это в `Awake`.
- Runtime-файл безусловно содержит `using UnityEditor` и вызывает `PrefabUtility` под conditional block. `using UnityEditor` тоже следует закрыть `#if UNITY_EDITOR` или убрать; иначе возможна ошибка Player build.
- `DestroyImmediate` вызывается и во время Play Mode; обычно runtime-объекты следует удалять через `Destroy`.
- Нет защит от `Default == null`, `levels == null`, пустого списка и некоторых выходов индекса за диапазон.
- `Level.playerSpawnPoint` не имеет публичного доступа и нигде не применяется.

Не исправлять менеджер вслепую без проверки требуемого поведения, но перечисленные дефекты учитывать обязательно.

## 7. Состояние сборки и проверка

Выполненная проверка:

```bash
dotnet build Runner_test_tusk.sln --no-restore
```

Повторная проверка после установки Unity MCP: сборка завершается с 0 errors и 4 `MSB3277` warnings о конфликтующих версиях сборок, добавленных `com.ivanmurzak.unity.mcp`. Unity Editor после импорта MCP не показывает свежих `error CS`, `NullReferenceException`, `MissingReferenceException` или `UnassignedReferenceException` в просмотренном хвосте лога. Это **не заменяет Unity Player build и PlayMode-прогон**; Unity MCP сейчас требует Cloud-авторизацию, поэтому автоматический live gameplay test не выполнен.

После изменений необходимо минимум:

1. открыть проект именно в Unity `6000.2.6f2`;
2. проверить Console на compile/serialization/missing-script ошибки;
3. запустить Level_1 в Play Mode и проверить mouse + touch drag;
4. проверить весь loop: start, pickups обоих типов, шкала/модели, obstacle/lose, flags/gates, finish/win, restart/next level;
5. проверить запуск Level_1 как build index 0 и сделать Development Build целевой платформы;
6. проверить UI на мобильном aspect ratio и safe area;
7. повторить `dotnet build` только как быструю дополнительную проверку C#.

## 8. Правила для дальнейшей работы

- Основной код и новые gameplay assets размещать под `Assets/Project/`, не в импортированных asset packs.
- Editor-only код держать в папке `Editor` и не допускать ссылок на `UnityEditor` из Player runtime assembly.
- Не редактировать `Library`, `Temp`, `Logs`, `obj`, `.vs`, generated `.csproj`/`.sln`.
- При перемещении/переименовании Unity assets сохранять соответствующие `.meta`, иначе сломаются GUID-ссылки.
- Не удалять большой импортированный набор до аудита реальных зависимостей сцен/prefab.
- Использовать сериализованные ссылки с валидацией (`Awake`/`OnValidate`) и избегать скрытых глобальных зависимостей.
- Состояния игры (pre-start/running/win/lose) должны централизованно блокировать движение и input.
- Управление сначала сравнить с референсом: sensitivity, зависимость от разрешения/DPI, абсолютный или относительный drag, инерция, ограничения дорожки и поведение на поворотах.
- Для мобильного ввода предпочтительно использовать уже подключённую новую Input System и `Assets/InputSystem_Actions.inputactions`; не смешивать два input backend без явной причины.
- Добавить хотя бы EditMode-тесты для level-index/progress logic и PlayMode smoke-тесты для state transitions/pickups.

## 9. Установленные инструменты Pi/Unity

- В `.pi/settings.json` project-local установлены `@aefree/pi-unity` и `@aefree/pi-unity-docs`.
- Глобально для Pi установлен `pi-mcp-adapter`.
- Глобально через npm установлен `unity-mcp-cli` версии `0.91.0`.
- В `Packages/manifest.json` установлен Unity package `com.ivanmurzak.unity.mcp` версии `0.91.0` и добавлен OpenUPM registry; пакет успешно разрешён в `Packages/packages-lock.json` и импортирован Unity.
- Unity MCP по умолчанию выбрал Cloud endpoint и требует авторизацию пользователя. До входа сервер отвечает HTTP 401, поэтому live MCP tools пока недоступны. Завершить настройку следует через окно **AI Game Developer** в Unity или `unity-mcp-cli login`, затем проверить `unity-mcp-cli status .`.
- Для `pi-unity-docs` база ещё не создана: offline Documentation для Unity `6000.2.6f2` в Editor install отсутствует. После установки документации запустить `/unity-docs-configure`.
- После установки/изменения Pi packages выполнить `/reload` или перезапустить Pi.

## 10. Рекомендуемый порядок завершения

1. Зафиксировать точное поведение референса и видео; описать состояния/значения шкалы и пороги моделей.
2. Проверить bootstrap/game state и запуск Level_1 из актуальных Build Settings.
3. Исправить движение, mobile drag и прохождение поворотов; затем камеру.
4. Сделать pickup/obstacle/flag/gate/finish компоненты и единую модель прогресса шкалы.
5. Подключить смену персонажа, Animator и SFX/VFX.
6. Реализовать стартовый/gameplay/win/lose/tutorial UI.
7. Исправить и интегрировать LevelManager, создать LevelsList и минимум два перехода (можно дублировать первый уровень согласно ТЗ).
8. Провести mobile build/playtest, polish, записать видео, затем подготовить чистый GitHub-репозиторий без generated данных.
