(defun C:ADD_NETLOAD (/ x1 x2 x3)
					;загружаем DLL 
  (vl-load-com)
  ;(vl-cmdf "_netload" "C:/Users/Fishman.DB.CORP/YandexDisk/Работа АСКО/AUTOCAD_Плагины/02-07-2021_/Layer_Add/bin/Debug/Layer_Add.dll") 
 (vl-cmdf "_netload" "C:\Users\den91\YandexDisk\Работа АСКО\AUTOCAD_Плагины\02-07-2021_\Layer_Add\bin\Debug\Layer_Add.dll")
  ;- лиспом dll загружаем
  (alert "DLL Загружен")
)