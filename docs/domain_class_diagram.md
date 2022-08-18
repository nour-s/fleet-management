```mermaid
classDiagram
    Sack  "0..1" *-- "0..*" Package: Packages

    class Sack{
        +string Barcode
        +int DeliveryPoint

        +AddPackage(Package)
        +Unload(deliveryPoint)
        +UnloadPackage(bacode, deliveryPoint)
    }

    class Package{
        +string Barcode
        +int DeliveryPoint
        +int Desi
        +Sack Sack
        +Unload(deliveryPoint)
    }

```
