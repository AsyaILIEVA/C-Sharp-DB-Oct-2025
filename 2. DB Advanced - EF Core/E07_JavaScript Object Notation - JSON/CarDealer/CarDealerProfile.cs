using AutoMapper;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            // Here we need to register our mapping conventions


            // Serialization of data -> EXPORT from DB
            // Fetches Data from DB, prepares them for transfer (DTO)
            // and transfers the data into byte[] (sent over network, stored in file)
            // object[] -> byte[] (string)

            // Deserialization of data -> IMPORT into DB
            // We receive data from outside, parse it, prepares them for transfer (DTO)
            // and eventually we store them in DB

        }
    }
}
