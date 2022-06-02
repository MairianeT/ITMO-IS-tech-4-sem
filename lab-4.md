# Отчет lab-4
## Запуск на 100 точках:

  Работа изначальной программы
 ![1](https://user-images.githubusercontent.com/79722210/168258121-986041a0-8449-43cf-901e-6d5e07d67e7c.png)

  В методе NewWay происходит замена всего пути для каждного гена. Сделаем замену только последней точки
  ![2](https://user-images.githubusercontent.com/79722210/168259004-efa0513a-5e3b-48a6-96de-fb042925f41a.png)
  Изменения по времени в методе NewWay не большие, тк мы вызываем PointWay.Count при каждной итерации. Можно задать новую переменную для хранения количества шагов в пути. Второй вариант: тк нам для дальнейшего хода не нужно знать, где находился ген ранее, можно весь "хвост" заменить на координаты 
  
  В классе PointMap заменим хранение всего пути каждого гена на координаты, где он находится на данный момент
![3](https://user-images.githubusercontent.com/79722210/168259531-cc3af672-6726-4484-93bd-9103a2466245.png)
  
  Время метода NewWay сократилось с 0.09мс до 0.001мс, время NextStep с 2.2мс до 1.1мс
  
  
  
  ## Аналогичные запуски на 1000 течках:
  ![4](https://user-images.githubusercontent.com/79722210/168260909-3863b9dd-b257-4c5f-a5a2-9a9f2f8f5476.png)
  ![5](https://user-images.githubusercontent.com/79722210/168260978-01aca2df-ba29-4510-81ba-fa109a4b1850.png)
  ![6](https://user-images.githubusercontent.com/79722210/168260999-e5703c5a-b90b-454a-a01d-7b4e35d488ac.png)
  
  
  
  ## Изменения памяти:
  1000 точек
  ![11](https://user-images.githubusercontent.com/79722210/168261411-2cb598ff-f03a-4710-9ff8-9fb5757ea207.jpg)
  ![12](https://user-images.githubusercontent.com/79722210/168261428-5e87db19-9f65-44d2-925d-e67c0b82f4eb.jpg)

  100 точек
  ![13](https://user-images.githubusercontent.com/79722210/168263558-7f05d008-702a-4411-acf4-267c7b0b10d9.jpg)
  ![14](https://user-images.githubusercontent.com/79722210/168263604-b53687d1-0013-45df-9ed4-180ba4229994.jpg)
  
  10к точек (без UI)
  ![31](https://user-images.githubusercontent.com/79722210/168289696-6e5c1136-6176-450f-afbc-e7151ffd7c6c.jpg)
  ![32](https://user-images.githubusercontent.com/79722210/168289716-f2e05d71-c4cd-4553-a4a4-fa991647447d.jpg)



  ## Сравнение на больших значениях без UI:
  100 точек
  ![21](https://user-images.githubusercontent.com/79722210/168287774-03c44c19-7fb9-4e3d-9e12-58cb66ea0478.jpg)
  ![22](https://user-images.githubusercontent.com/79722210/168287794-d510ce2f-e36c-4349-b0df-422ea23c04b1.jpg)
  
  
  10к точек
  ![23](https://user-images.githubusercontent.com/79722210/168287918-6507fe77-0d15-470f-9ffb-6ba8c2c12f4d.jpg)
  ![24](https://user-images.githubusercontent.com/79722210/168287933-4a5a25bb-c185-4d25-8de9-df1a447ba252.jpg)
  
  100к
  ![25](https://user-images.githubusercontent.com/79722210/168288354-997818a4-5307-48af-bf18-359a165d415b.jpg)
  ![26](https://user-images.githubusercontent.com/79722210/168288367-bee6e8a2-5a4b-48d8-b44b-271e6bdf6a6e.jpg)
