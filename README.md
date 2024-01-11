# BlackRed
Проект является курсовой работы по базам данных. Задумка состояла в создании прототипа игры, в которой игроки соревновались бы друг с другом в скорости прохождения уровней, 
по средством ассинхронного мультиплеера.

Серверная часть: https://github.com/workavast/BlackRed_Backend

## Что было реализовано

- **Клиен-серверное взаимодействие**:

  - **WebApi** – отвечает за формирование и отправку запроса серверу

  - **ApiBase** – отвечает за формирование адреса запроса, его отправку через 
  WebApi, а также за парсинг ошибки и вызов делегата, реагирующего на ошибку, в случае ее возникновения

      Пример кода ApiBase:
    
        protected async Task<(bool, T)> TryGet<T>(string methodPath, Action<string> onError, string jwt = null)
        {
            string fullPath = $"{MainPartOfPath}/{ControllerPath}/{methodPath}";
            
            var res = await WebApi.Get<T>(fullPath, jwt);
            if (!res.Item1)
                onError?.Invoke(WebApiParse.ErrorResult(res.Item2));

            return (res.Item1, res.Item3);
        }

  - **Классы наследники ApiBase** – отвечают за формирование данных запроса и действия после получения результата от сервера и вызов делегата, реагирующего на успешный запрос

      Пример кода AuthenticationApi:

        public async void UserRegistration(Action onSuccess, Action<string> onError, string playerName, string playerPassword)
        {
            var req = new AuthenticationRequest(playerName, playerPassword);
            
            var res = await TryPost<AuthenticationResponse>(AuthenticationControllerPaths.Register, req, onError);
            if(!res.Item1) return;
            
            PlayerDataStorage.SetMainData(playerName, playerPassword, res.Item2.Toke);
            
            onSuccess?.Invoke();
        }
  
- **GhostSystem** — отвечает за запись и воспроизведение «призраков» игроков. Для минимизации кол-ва точек, на основе которых воссоздаётся путь «призрака»,
  запись точек производиться только при смене направления движения и/или при изменении скорости движения. Точка представляет собой координаты x, y, z и момент времени от начала прохождения уровня,
  когда игрок находился в них.
  
  Красная сфера это "призрак" текущего игрока, т.е. она движется по тому же пути что и игрок в своём наилучшем прохождении уровня.

  Синии сферы это "призраки" других игроков, которые имеют лучше время прохождения уровня чем игрок.

  ![MainDemonstration](https://github.com/workavast/BlackRed/assets/90834653/79d1bfd1-b248-4e2e-8705-83a0f7d0dcae)

- **UISystem** — отвечает за взаимодействие игрока с интерфейсом. Из интерфейса были реализованы стартовое меню, главное меню, экран выбора уровня, экран друзей, меню во время геймплея,
  экран результата прохождения уровня, вывод сообщений об ошибке на сервере.

  Экран выбора уровня, с выводом глобально рейтинга и рейтинга среди друзей:
  ![LevelChoiseScreenDemonstration (1)](https://github.com/workavast/BlackRed/assets/90834653/d5053e68-6f22-400a-9650-571cc0803ba1)

  Экран друзей:
  ![FriendsScreenDemonstration (1)](https://github.com/workavast/BlackRed/assets/90834653/61cf2eeb-5de7-4687-8ce8-3a47522e249a)

- **Основное управление** — игрок может двигаться, прыгать, приседать, делать подкат, соскальзывать с наклонных поверхностей под большим углом.
