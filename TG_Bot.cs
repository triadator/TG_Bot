using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Models;
using System.Text.RegularExpressions;
using TelegramBot.Models.DbSets;
using TelegramBot.Enums;
using ConsoleApp1;

namespace TelegramBot
{
    public class TG_Bot
    {
        static string TELEGRAM_TOKEN = "5952579553:AAGpMn-35VMV1zJcSSfejzmIHnGrIwto5hg";
              

        public  TG_Bot()
        {
            var botClient = new TelegramBotClient(TELEGRAM_TOKEN);
            using var cts = new CancellationTokenSource();
            var recieverOptions = new ReceiverOptions()

            {
                AllowedUpdates = { }
            };
            botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, recieverOptions, cancellationToken: cts.Token);
#if DEBUG
            Console.WriteLine("[Log]: Bot started");
#endif
        }
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errormessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(errormessage);
            return Task.CompletedTask;
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message && update.Message!.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                await HandleMessage(botClient, update.Message, cancellationToken);
            }

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery) 
            {
                await HandleCallbackQuery(botClient, update.CallbackQuery);
            }
        }
        static async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery c)
        {

            var action = (c.Data) switch
            {
                #region Настройка размеров одежды
                "Верхняя одежда" => MyOverDressHandler(botClient, c),
                "SetOverDressSize 44-46" => SetSizeInfo(botClient, c, "44-46", TypeOfClothesEnum.Пальто),
                "SetOverDressSize 48-50" => SetSizeInfo(botClient, c, "48-50", TypeOfClothesEnum.Пальто),
                "SetOverDressSize 52-54" => SetSizeInfo(botClient, c, "52-54", TypeOfClothesEnum.Пальто),
                "SetOverDressSize 56-58" => SetSizeInfo(botClient, c, "56-58", TypeOfClothesEnum.Пальто),

                "Платья" => MyDressHandler(botClient, c),
                "SetDressSize 44-46" => SetSizeInfo(botClient, c, "44-46", TypeOfClothesEnum.Платье),
                "SetDressSize 48-50" => SetSizeInfo(botClient, c, "48-50", TypeOfClothesEnum.Платье),
                "SetDressSize 52-54" => SetSizeInfo(botClient, c, "52-54", TypeOfClothesEnum.Платье),
                "SetDressSize 56-58" => SetSizeInfo(botClient, c, "56-58", TypeOfClothesEnum.Платье),

                "Блузки/Свитера" => MyTShortSizeHandler(botClient, c),
                "SetTShortSize 44-46" => SetSizeInfo(botClient, c, "44-46", TypeOfClothesEnum.Блуза),
                "SetTShortSize 48-50" => SetSizeInfo(botClient, c, "48-50", TypeOfClothesEnum.Блуза),
                "SetTShortSize 52-54" => SetSizeInfo(botClient, c, "52-54", TypeOfClothesEnum.Блуза),
                "SetTShortSize 56-58" => SetSizeInfo(botClient, c, "56-58", TypeOfClothesEnum.Блуза),
                "Брюки" => MyTrousersSizeHandler(botClient, c),
                "SetTrousersSize 44-46" => SetSizeInfo(botClient, c, "44-46", TypeOfClothesEnum.Брюки),
                "SetTrousersSize 48-50" => SetSizeInfo(botClient, c, "48-50", TypeOfClothesEnum.Брюки),
                "SetTrousersSize 52-54" => SetSizeInfo(botClient, c, "52-54", TypeOfClothesEnum.Брюки),
                "SetTrousersSize 56-58" => SetSizeInfo(botClient, c, "56-58", TypeOfClothesEnum.Брюки),
                //  _ => NotExpectedCallbackQueryHandler
                #endregion
                #region Выдача фото коллекции
                "ShowAllOverdress" => ShowAllDressHandler(botClient, c, typeOfClothesEnum: TypeOfClothesEnum.Пальто),
                "ShowAllDress" => ShowAllDressHandler(botClient, c, typeOfClothesEnum: TypeOfClothesEnum.Платье),
                "ShowAllTShorts" => ShowAllDressHandler(botClient, c, typeOfClothesEnum: TypeOfClothesEnum.Блуза),
                "ShowAllTrousers" => ShowAllDressHandler(botClient, c, typeOfClothesEnum: TypeOfClothesEnum.Брюки),
                #endregion
                #region Выдача фото новой коллекции 
                "ShowNewOverdress" => ShowNewDressHandler(botClient, c, typeOfClothesEnum: TypeOfClothesEnum.Пальто),
                "ShowNewDress" => ShowNewDressHandler(botClient, c, typeOfClothesEnum: TypeOfClothesEnum.Платье),
                "ShowNewTShorts" => ShowNewDressHandler(botClient, c, typeOfClothesEnum: TypeOfClothesEnum.Блуза),
                "ShowNewTrousers" => ShowNewDressHandler(botClient, c, typeOfClothesEnum: TypeOfClothesEnum.Брюки),
                #endregion
                #region Выдача фото коллекции по размеру
                "ShowMyOverdresses" => ShowMyDressHandler(botClient, c, typeOfClothesEnum: TypeOfClothesEnum.Пальто),
                "ShowMyDresses" => ShowMyDressHandler(botClient, c, typeOfClothesEnum: TypeOfClothesEnum.Платье),
                "ShowMyTShorts" => ShowMyDressHandler(botClient, c, typeOfClothesEnum: TypeOfClothesEnum.Блуза),
                "ShowMyTrousers" => ShowMyDressHandler(botClient, c, typeOfClothesEnum: TypeOfClothesEnum.Брюки),
                #endregion
            };
            //SetsizeHandlers          
            static async Task<Message> MyOverDressHandler(ITelegramBotClient botClient, CallbackQuery c)
            {
                InlineKeyboardMarkup inlineKeyboard = new(new[]
                                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text:"44-46",callbackData:"SetOverDressSize 44-46"),
                        InlineKeyboardButton.WithCallbackData(text:"48-50",callbackData:"SetOverDressSize 48-50")

                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text:"52-54",callbackData:"SetOverDressSize 52-54"),
                        InlineKeyboardButton.WithCallbackData(text:"56-58",callbackData:"SetOverDressSize 56-58")
                    }
                });
                return await botClient.SendTextMessageAsync(
                   chatId: c.Message.Chat.Id,
                   text: "Выберите размер верхней одежды",
                   replyMarkup: inlineKeyboard);

            }
            static async Task<Message> MyDressHandler(ITelegramBotClient botClient, CallbackQuery c)
            {
                InlineKeyboardMarkup inlineKeyboard = new(new[]
                                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text:"44-46",callbackData:"SetDressSize 44-46"),
                        InlineKeyboardButton.WithCallbackData(text:"48-50",callbackData:"SetDressSize 48-50")

                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text:"52-54",callbackData:"SetDressSize 52-54"),
                        InlineKeyboardButton.WithCallbackData(text:"56-58",callbackData:"SetDressSize 56-58")
                    }
                });
                return await botClient.SendTextMessageAsync(
                   chatId: c.Message.Chat.Id,
                   text: "Выберите размер одежды",
                   replyMarkup: inlineKeyboard);

            }
            static async Task<Message> MyTShortSizeHandler(ITelegramBotClient botClient, CallbackQuery c)
            {
                InlineKeyboardMarkup inlineKeyboard = new(new[]
                                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text:"44-46",callbackData:"SetTShortSize 44-46"),
                        InlineKeyboardButton.WithCallbackData(text:"48-50",callbackData:"SetTShortSize 48-50")

                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text:"52-54",callbackData:"SetTShortSize 52-54"),
                        InlineKeyboardButton.WithCallbackData(text:"56-58",callbackData:"SetTShortSize 56-58")
                    }
                });
                return await botClient.SendTextMessageAsync(
                   chatId: c.Message.Chat.Id,
                   text: "Выберите размер блузы/свитера",
                   replyMarkup: inlineKeyboard);

            }
            static async Task<Message> MyTrousersSizeHandler(ITelegramBotClient botClient, CallbackQuery c)
            {
                InlineKeyboardMarkup inlineKeyboard = new(new[]
                                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text:"44-46",callbackData:"SetTrousersSize 44-46"),
                        InlineKeyboardButton.WithCallbackData(text:"48-50",callbackData:"SetTrousersSize 48-50")

                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text:"52-54",callbackData:"SetTrousersSize 52-54"),
                        InlineKeyboardButton.WithCallbackData(text:"56-58",callbackData:"SetTrousersSize 56-58")
                    }
                });
                return await botClient.SendTextMessageAsync(
                   chatId: c.Message.Chat.Id,
                   text: "Выберите размер брюк",
                   replyMarkup: inlineKeyboard);

            }
            static async Task<Message> SetSizeInfo(ITelegramBotClient botClient, CallbackQuery c, string size, TypeOfClothesEnum typeOfClothesEnum)
            {
                using (BotContext db = new())
                {
                    var g = db.Clients.FirstOrDefault(g => g.TelegramId == c.From.Id);
                    var x = db.ClientSizeInfo.FirstOrDefault(x => x.ClientId == g.Id);
                    switch (typeOfClothesEnum)
                    {
                        case TypeOfClothesEnum.Пальто:
                            x.OverDressSize = size;
                            break;
                        case TypeOfClothesEnum.Платье:
                            x.DressSize = size;
                            break;
                        case TypeOfClothesEnum.Блуза:
                            x.T_ShortSize = size;
                            break;
                        case TypeOfClothesEnum.Брюки:
                            x.TrousersSize = size;
                            break;
                    }
                    db.SaveChanges();

                }
                return await botClient.SendTextMessageAsync(
                  chatId: c.Message.Chat.Id,
                  text: "Размер закреплен в Вашем профиле.");
            }
            static async Task<Message> NotExpectedCallbackQueryHandler(ITelegramBotClient botClient, CallbackQuery c)
            {

                return await botClient.SendTextMessageAsync(chatId: c.Message.Chat.Id,
                    $"Ошибка CallbackQuery");

            }
            //
            static async Task ShowAllDressHandler(ITelegramBotClient botClient, CallbackQuery c, TypeOfClothesEnum typeOfClothesEnum)
            {
                {
                    var currentItemType = Enum.GetName(typeof(TypeOfClothesEnum), typeOfClothesEnum);
                    IEnumerable<Item> items;

                    using (BotContext db = new ())
                    {
                        items = db.Items.Where(x => x.Type == currentItemType).ToList();

                    }
                    foreach (var item in items.DistinctBy(x => x.Barcode))
                    {
                        await botClient.SendPhotoAsync(chatId: c.Message.Chat.Id,
                                                       photo: item.PhotoUrl,
                                                       caption: $"{item.Type} {item.Name}\n" +
                                                       $"Цвет: {item.Color}");
                    }
                    // ReplyKeyboardMarkup keyboard = new(new[]
                    //{
                    //     new KeyboardButton[] { "Показать еще" }
                    // });                   

                    // await botClient.SendTextMessageAsync(chatId: c.Message.Chat.Id,
                    //     $"",
                    //     replyMarkup: keyboard);

                }
            }
            static async Task ShowNewDressHandler(ITelegramBotClient botClient, CallbackQuery c, TypeOfClothesEnum typeOfClothesEnum)
            {
                var newColletion = DateTime.Parse("13/01/2023 16:10:00");
                var currentItemType = Enum.GetName(typeof(TypeOfClothesEnum), typeOfClothesEnum);
                IEnumerable<Item> items;

                using (BotContext db = new())
                {
                    items = db.Items.Where(x => (x.Type == currentItemType) & (x.CreationDate > newColletion)).ToList();

                }
                foreach (var item in items.DistinctBy(x => x.Barcode))
                {
                    await botClient.SendPhotoAsync(chatId: c.Message.Chat.Id,
                                                    photo: item.PhotoUrl,
                                                    caption: $"{item.Type} {item.Name}\n" +
                                                    $"Цвет: {item.Color}\n"+
                                                    $"Новая желтая коллекция!");
                }
                //ReplyKeyboardMarkup keyboard = new(new[] 
                //{
                //    new KeyboardButton[] { "Показать еще" }
                //});

                //await botClient.SendTextMessageAsync(chatId: c.Message.Chat.Id,
                //    $"",
                //    replyMarkup: keyboard);


            }
            static async Task ShowMyDressHandler(ITelegramBotClient botClient, CallbackQuery c, TypeOfClothesEnum typeOfClothesEnum)
            {
                var currentItemType = Enum.GetName(typeof(TypeOfClothesEnum), typeOfClothesEnum);
                List<Item> items;

                Client currentClient;
                ClientSizeInfo clientSizeInfo;

                using (BotContext db = new())
                {
                    currentClient = db.Clients.FirstOrDefault(x => x.TelegramId == c.From.Id);
                    clientSizeInfo = db.ClientSizeInfo.FirstOrDefault(x => x.ClientId == currentClient.Id);
                    switch (typeOfClothesEnum)
                    {
                        case TypeOfClothesEnum.Пальто:
                            items = db.Items.Where(x => (x.Type == currentItemType) & (x.Size == clientSizeInfo.OverDressSize)&(x.Leftover>0)).ToList();
                            break;
                        case TypeOfClothesEnum.Платье:
                            items = db.Items.Where(x => (x.Type == currentItemType) & (x.Size == clientSizeInfo.DressSize) & (x.Leftover > 0)).ToList();
                            break;
                        case TypeOfClothesEnum.Блуза:
                            items = db.Items.Where(x => (x.Type == currentItemType) & (x.Size == clientSizeInfo.T_ShortSize) & (x.Leftover > 0)).ToList();
                            break;
                        default:
                            items = db.Items.Where(x => (x.Type == currentItemType) & (x.Size == clientSizeInfo.TrousersSize) & (x.Leftover > 0)).ToList();
                            break;                        
                    }   
                    

                }
                foreach (var item in items.DistinctBy(x => x.Barcode))
                {
                    await botClient.SendPhotoAsync(chatId: c.Message.Chat.Id,
                                                   photo: item.PhotoUrl,
                                                   caption: $"{item.Type} {item.Name}\n" +
                                                   $"Цвет: {item.Color}. Размер: {item.Size}");
                }



            }
                  

        }
        static async Task HandleMessage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            if (message.ReplyToMessage != null)
            {
                var replyAction = (message.ReplyToMessage.Text) switch
                {
                    "Укажите cвой e-mail" => EmailRegistrationHandler(botClient, message),
                    "Вы указали e-mail с ошибкой.\nДавайте попробует еще раз." => EmailRegistrationHandler(botClient, message),
                    $"Почта закреплена за Вашим аккаунтом.\nТеперь укажите свой телефон" => PhoneRegistrationHandler(botClient, message),
                    $"Напишите свой отзыв о работе магазина.\nПо умолчанию отзыв является анонимным,\n" +
                    $"Однако Вы вправе оставить свои контактные \nданные для обратной связи." => EmailHandler(botClient, message, TypeOFEmailEnum.Feedback),
                    $"Кратко укажите информацию о товаре и его недостатках,\n" +
                    $"выявленных в течение гарантийного срока.\n"+
                    $"Представитель магазина свяжется с Вами в течение\n" +
                    $"3 рабочих дней по телефону, уканному\n" +
                    $"при регистрации в личном кабинете." => EmailHandler(botClient, message, TypeOFEmailEnum.Complaint)
                };
            }
            else
            {
                try
                {
                    var action = (message.Text) switch
                    {
                        "/start" => StartHandler(botClient, message),
                        "/help"=> HelpHandler(botClient,message),
                        "/feedback" => FeedbackHandler(botClient, message),
                        "Подать жалобу" => ComplaintHandler(botClient, message),
                        "/shopphone" => GetPhoneContactHandler(botClient, message),
                        "/shoponthemap" => GetAdressContactHandler(botClient, message),                    
                        "Личный кабинет" => MyAccountHandler(botClient, message),
                        "Назад в личный кабинет" => MyAccountHandler(botClient, message),
                        "Пройти регистрацию" => RegistrationHandler(botClient, message),
                        "Настроить мои размеры" => SizeSettingsHandler(botClient, message),
                        "⬅️ Меню" => GoBackButtonHandler(botClient, message),
                        "Каталог" => StockHandler(botClient, message),
                        "Весь ассортимент" => AllStockHandler(botClient, message),
                        "Подобрать одежду по размеру" => MyStockHandler(botClient, message),
                        "Новая коллекция" => NewStockHandler(botClient, message),
                        "История покупок" => HistoryHandler(botClient,message),
                        
                        _ => NotExpectedMessageHandler(botClient, message),
                    };
                }
                catch (Exception)
                {                    
                    throw;
                }
            }             
            

            static async Task<Message> StartHandler(ITelegramBotClient botClient, Message message)
            {
                ReplyKeyboardMarkup keyboard = new(new[]
                   {
                        new KeyboardButton[] { "Каталог", "Личный кабинет" }                        
                    });
                keyboard.ResizeKeyboard = true;

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                    $"Добрый день, {message.From.FirstName}.\nЭто телеграм-бот магазина {"Суаре"}.\nВыбери команду",
                    replyMarkup: keyboard);
                             
            }
            static async Task<Message> HelpHandler(ITelegramBotClient botClient, Message message)
            {                
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                    $"В нашем боте Вы можете получить информацию о:" +
                    $"\n-контактных данных магазина" +
                    $"\n-товаре, имеющиемся в наличии" +
                    $"\n-истории Ваших покупок (доступно после регистрации)" +
                    $"\n-правилах ухода за купленным товаром." +
                    $"\nКроме того Вы можете связаться с представителем магазина для разрешения " +
                    $"споров о покупке товара ненадлежащего качества (доступно после регистрации) " +
                    $"или направлить предложения по улучшению работы бота и магазина");

            }            
            static async Task<Message> GetPhoneContactHandler(ITelegramBotClient botClient, Message message)
            {
                return await botClient.SendContactAsync(
                      chatId: message.Chat.Id,
                      phoneNumber: "+1234567890",
                      firstName: "Магазин Суаре",
                      vCard: "BEGIN:VCARD\n" +
                      "VERSION:3.0\n" +
                      "N:Solo;Han\n" +
                      "TEL;TYPE=voice,магазин,pref:+1234567890\n" +
                      "EMAIL:soiree@gmail.com\n" +
                      "END:VCARD");
            }
            static async Task<Message> GetAdressContactHandler(ITelegramBotClient botClient, Message message)
            {
               return await botClient.SendLocationAsync(
                    chatId: message.Chat.Id,
                    latitude: 60.005306f,
                    longitude: 30.262572f);

               return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                    $"Комендантский проспект, 9/2\nСанкт - Петербург, 197227" +
                    $"\nТРК {"Променад"}, первый этаж." +
                    $"\nЧасы работы: 10:00 - 22:00");
            }
            static async Task<Message> MyAccountHandler(ITelegramBotClient botClient, Message message)
            {
                object registredClient;
                using (BotContext db = new())
                {
                    registredClient = db.Clients.FirstOrDefault(x => x.TelegramId == message.From.Id);
                    
                }
                if (registredClient != null)
                {
                    ReplyKeyboardMarkup keyboard = new(new[]
                  {
                        new KeyboardButton[] { "Настроить мои размеры", "Подать жалобу"},
                        new KeyboardButton[] { "История покупок", "⬅️ Меню" }
                      });
                    keyboard.ResizeKeyboard = true;
                    return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                    "Добро пожаловать в личный кабинет.", replyMarkup: keyboard);
                }
                else 
                {
                    ReplyKeyboardMarkup keyboard = new(new[]
                    {
                        new KeyboardButton[] { "Пройти регистрацию"},
                        new KeyboardButton[] { "⬅️ Меню" }
                    });
                    keyboard.ResizeKeyboard = true;

                    return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                   "Для использования личного кабинета\n" +
                   "Вам необходимо проийти короткую регистрацию.\n" +
                   "Проходя регистрацию Вы даете согласие\n" +
                   "На использование Ваших персональных данных.", replyMarkup: keyboard);
                }
                
            }
            static async Task<Message> SizeSettingsHandler(ITelegramBotClient botClient, Message message)
            {
                InlineKeyboardMarkup inlineKeyboard = new(new[]
                                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(textAndCallbackData:"Верхняя одежда"),
                        InlineKeyboardButton.WithCallbackData(textAndCallbackData:"Блузки/Свитера")
                                                
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(textAndCallbackData:"Платья"),
                        InlineKeyboardButton.WithCallbackData(textAndCallbackData:"Брюки")
                    }
                });
                

                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Выберите категорию:",
                    replyMarkup: inlineKeyboard);
            }
            static async Task<Message> RegistrationHandler(ITelegramBotClient botClient, Message message)
            {
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                             $"Укажите cвой e-mail",
                replyMarkup: new ForceReplyMarkup{ Selective = true, InputFieldPlaceholder = "Напишите сюда свой e-mail" });
            }
            static async Task<Message> GoBackButtonHandler(ITelegramBotClient botClient, Message message)
            {
                ReplyKeyboardMarkup keyboard = new(new[]
                    {
                        new KeyboardButton[] { "Каталог", "Личный кабинет" }
                    });
                keyboard.ResizeKeyboard = true;

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                    $"Вы в главном меню.",
                    replyMarkup: keyboard);
            }
            static async Task<Message> StockHandler(ITelegramBotClient botClient, Message message)
            {
                ReplyKeyboardMarkup keyboard = new(new[]
                   {
                        new KeyboardButton[] { "Весь ассортимент", "Подобрать одежду по размеру" },
                        new KeyboardButton[] { "Новая коллекция",  "⬅️ Меню" }
                    });
                keyboard.ResizeKeyboard = true;

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            $"Что бы Вы хотели посмотреть?",
                                                            replyMarkup: keyboard);
               
            }
            static async Task<Message> AllStockHandler(ITelegramBotClient botClient, Message message)
            {
                InlineKeyboardMarkup inlineKeyboard = new(new[]
                               {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Верхняя одежда",callbackData:"ShowAllOverdress"),
                        InlineKeyboardButton.WithCallbackData(text:"Платья",callbackData:"ShowAllDress")

                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Блузы/Свитера",callbackData:"ShowAllTShorts"),
                        InlineKeyboardButton.WithCallbackData(text:"Брюки",callbackData:"ShowAllTrousers")
                    }
                });
                return await botClient.SendTextMessageAsync(
                   chatId: message.Chat.Id,
                   text: "Выберите нужную опцию:",
                   replyMarkup: inlineKeyboard);
                
                
            }
            static async Task<Message> MyStockHandler(ITelegramBotClient botClient, Message message)
            {
                object registredClient;
                using (BotContext db = new())
                {
                    registredClient = db.Clients.FirstOrDefault(x => x.TelegramId == message.From.Id);
                }
                if (registredClient == null)
                {
                    ReplyKeyboardMarkup keyboard = new(new[]
               {
                        new KeyboardButton[] { "Весь ассортимент", "Пройти регистрацию" },
                        new KeyboardButton[] { "Новая коллекция",  "⬅️ Меню" }
                    });
                    keyboard.ResizeKeyboard = true;

                   return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                  "Для подбора одежды по размеру Вам необходимо \n" +
                  "проийти короткую регистрацию в личном кабинете.\n" +
                  "Проходя регистрацию Вы даете согласие\n" +
                  "на использование Ваших персональных данных.", replyMarkup: keyboard);
                }
                else 
                {
                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                              {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Верхняя одежда",callbackData:"ShowMyOverdresses"),
                        InlineKeyboardButton.WithCallbackData(text:"Платья",callbackData:"ShowMyDresses")

                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Блузы/Свитера",callbackData:"ShowMyTShorts"),
                        InlineKeyboardButton.WithCallbackData(text:"Брюки",callbackData:"ShowMyTrousers")
                    }
                });
                    return await botClient.SendTextMessageAsync(
                       chatId: message.Chat.Id,
                       text: "Выберите нужную опцию:",
                       replyMarkup: inlineKeyboard);
                   
                }
               
            }
            static async Task<Message> NewStockHandler(ITelegramBotClient botClient, Message message)
            {
                InlineKeyboardMarkup inlineKeyboard = new(new[]
                               {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Верхняя одежда",callbackData:"ShowNewOverdress"),
                        InlineKeyboardButton.WithCallbackData(text:"Платья",callbackData:"ShowNewDress")

                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Блузы/Свитера",callbackData:"ShowNewTShorts"),
                        InlineKeyboardButton.WithCallbackData(text:"Брюки",callbackData:"ShowNewTrousers")
                    }
                });
                return await botClient.SendTextMessageAsync(
                   chatId: message.Chat.Id,
                   text: "Выберите нужную опцию:",
                   replyMarkup: inlineKeyboard);
            }
            static async Task<Message> EmailRegistrationHandler(ITelegramBotClient botClient, Message message)
            {
                if (Regex.IsMatch(message.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    await using (BotContext db = new())
                    {
                        Client client = new ()
                        {
                            Email = message.Text,
                            TelegramId = message.From.Id,
                            Lastname = message.From.LastName,
                            Name = message.From.FirstName,
                            SizeInfo = new ClientSizeInfo() { }
                          
                        };
                        db.Clients.Add(client);                                               
                        db.SaveChanges();
                    }

                   return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                $"Теперь укажите свой телефон",
                                replyMarkup: new ForceReplyMarkup
                                {
                                    Selective = true,
                                    InputFieldPlaceholder = "Напишите сюда свой телефон"
                                });
                }
                else 
                {
                    return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                            $"Вы указали e-mail с ошибкой.\n" +
                            $"Давайте попробует еще раз.",
                            replyMarkup: new ForceReplyMarkup
                            {
                                Selective = true,
                                InputFieldPlaceholder = "Напишите сюда свой e-mail"
                            });
                }
            }           
            static async Task<Message> PhoneRegistrationHandler(ITelegramBotClient botClient, Message message)
            {
                await using (BotContext db = new())
                {
                    Client x = db.Clients.FirstOrDefault(x => x.TelegramId == message.From.Id);
                    x.Phone = message.Text;
                    db.SaveChanges();
                }

                ReplyKeyboardMarkup keyboard = new(new[]
               {
                        new KeyboardButton[] { "Настроить мои размеры", "Подать жалобу"},
                        new KeyboardButton[] { "Подобрать одежду по размеру", "История покупок" }
                      });
                keyboard.ResizeKeyboard = true;
               return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                           $"Регистрация завершена.\n",
                                                           replyMarkup: keyboard);
            }
            static async Task<Message> FeedbackHandler(ITelegramBotClient botClient, Message message)
            {
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                             $"Напишите свой отзыв о работе магазина.\n" +
                                                             $"По умолчанию отзыв является анонимным,\n" +
                                                             $"Однако Вы вправе оставить свои контактные \nданные для обратной связи.",
                replyMarkup: new ForceReplyMarkup { Selective = true });
            }
            static async Task<Message> ComplaintHandler(ITelegramBotClient botClient, Message message)
            {
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                             $"Кратко укажите информацию о товаре и его недостатках,\n" +
                                                             $"выявленных в течение гарантийного срока.\n"+
                                                             $"Представитель магазина свяжется с Вами в течение\n" +
                                                             $"3 рабочих дней по телефону, уканному\n" +
                                                             $"при регистрации в личном кабинете.",
                replyMarkup: new ForceReplyMarkup { Selective = true });
            }
            static async Task<Message> EmailHandler(ITelegramBotClient botClient, Message message, TypeOFEmailEnum typeOFEmailEnum)
            {

                ReplyKeyboardMarkup keyboard = new(new[]
               {new KeyboardButton[] {  "⬅️ Меню" }});
                keyboard.ResizeKeyboard = true;
                switch (typeOFEmailEnum)
                {
                    case TypeOFEmailEnum.Feedback:
                        EmailHelper.SendEmail(message.Text, Enum.GetName(TypeOFEmailEnum.Feedback));
                        break;

                    case TypeOFEmailEnum.Complaint:
                        Client client;                       
                        using (BotContext db = new())
                        {
                            client = db.Clients.FirstOrDefault(g => g.TelegramId == message.From.Id);                                              

                        }
                        string emailmessage = $"От {client.Lastname} {client.Name} {client.MiddleName}\n" +
                                              $"{client.Email} | {client.Phone}\n" +
                                              $"{message.Text}";
                        EmailHelper.SendEmail(emailmessage, Enum.GetName(TypeOFEmailEnum.Complaint));
                        break;
                    
                }
                               
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                        $"Спасибо за обратную связь! Ваше сообщение направлено на электронную почту магазина."
                        , replyMarkup: keyboard);
            }
            static async Task<Message> HistoryHandler(ITelegramBotClient botClient, Message message)
            {
                ReplyKeyboardMarkup keyboard = new(new[]
                   {
                        new KeyboardButton[] { "Назад в личный кабинет" }
                    });
                keyboard.ResizeKeyboard = true;

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                    $"История покупок отсутствует",
                    replyMarkup: keyboard);

            }

            static async Task<Message> NotExpectedMessageHandler(ITelegramBotClient botClient, Message message)
            {
                
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                    $"К сожалению боту неизвестна эта команда");

            }
        }
                                
    }
}
