using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SampleHotel.Infrastructure;
using SampleHotel.Models;
using SampleHotel.ViewModels;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SampleHotel.Controllers
{
    public class PanelController : Controller
    {
        private readonly DapperContext _context;
        public PanelController(DapperContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Employee(int id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            string sqlQuery = $@"SELECT e.[Id],[FirstName],[LastName],[PhoneNumber],[DependentFirstName],[DependentLastName],[DependentRelation]
                  ,[City],[Street],[EmployeePositionId],[ImageUrl], ep.Name AS EmployeePositionName
              FROM [SampleHotelDb].[dbo].[Employee] e
              inner join EmployeePosition ep on ep.Id = e.EmployeePositionId where e.Id = @id";

            using var connection = _context.CreateConnection();
            var employee = await connection.QuerySingleAsync<EmployeeDto>(sqlQuery, new { id });            

            return View(employee);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> FavoriteList()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            string sqlQuery = $@"SELECT r.[Id],[Floor],[Capacity],[ImageUrl],[HasJacuzzi],[HasBreakfast],[RoomTypeId]
                ,r.[Location],r.[Name]
                ,rt.Name as RoomTypeName FROM [SampleHotelDb].[dbo].[Room] r
                inner join RoomType rt on rt.Id = r.RoomTypeId
                inner join [dbo].[LikedRoom] lr on lr.RoomId = r.[Id] where lr.UserId = @userId";

            using var connection = _context.CreateConnection();
            var roomList = await connection.QueryAsync<RoomDto>(sqlQuery, new { userId });

            var roomVm = new AvailableRoomViewModel
            {
                RoomList = roomList,
            };

            return View(roomVm);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LikeRoom(int id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            string sql = @"INSERT INTO [dbo].[LikedRoom]([UserId],[RoomId])
                            VALUES(@userId, @roomId)";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, new
            {
                userId,
                roomId = id
            });

            return RedirectToAction("AvailableRoom");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UnLikeRoom(int id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            string sql = $@"Delete From [dbo].[LikedRoom] where RoomId = @roomId and UserId= @userId";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, new
            {
                userId,
                roomId = id
            });

            return RedirectToAction("AvailableRoom");
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateTicket()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public async Task <IActionResult> CreateTicket(CreateTicketViewModel model)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            string sql = @"INSERT INTO [dbo].[Ticket]([Title],[Priority],[Created],[Description],[Creator])
                        VALUES(@Title,@Priority,@Created,@Description,@Creator)";

            using var connection = _context.CreateConnection();
            
            await connection.ExecuteAsync(sql, new
            {
                model.Title,
                model.Priority,
                Created = DateTime.Now,
                model.Description,
                Creator = userId
            });                      

            return RedirectToAction("TicketList");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TicketList()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool isAdmin = User.Claims.Any(e => e.Value == "Admin");
            string sql = @$"SELECT [Id],[Title],[Priority],[Created],[Description],[Creator] FROM [dbo].[Ticket]
                            {(!isAdmin ? "Where Creator=@userId" : "" )}";
            
            using var connection = _context.CreateConnection();

            var vm = new TicketListViewModel()
            {
                CurrentUserId = userId,
            };

            vm.TicketList = await connection.QueryAsync<TicketDto>(sql, new { userId });

            foreach (var item in vm.TicketList)
            {
                sql = @"SELECT [Id],[Title],[TicketId],[Sender],[Created] FROM [dbo].[TicketMessage] where TicketId = @ticketId";
                item.TicketMessages = 
                    (await connection.QueryAsync<TicketMessageDto>(sql, new { ticketId = item.Id })).ToList();                
            }

            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendMessage(TicketListViewModel model, int id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            string sql = @"INSERT INTO [dbo].[TicketMessage]([Title],[TicketId],[Sender],[Created]) 
                           VALUES(@Title, @TicketId, @Sender, @Created)";

            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(sql, new
            {
                model.Message.Title,
                Sender = userId,   
                Created = DateTime.Now,
                TicketId = id,
            });

            model.Message = new TicketMessageDto();

            return RedirectToAction("TicketList");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string sqlQuery = @"SELECT [Id],[FullName],[Country],[City],[Street],[No],[UserName],[Email],[PhoneNumber]
  FROM [SampleHotelDb].[dbo].[AspNetUsers] where Id = @UserId";
            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QuerySingleOrDefaultAsync<ProfileViewModel>(sqlQuery, new { UserId = userId });
                return View(user);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AvailableRoom([FromQuery] RoomSearchParameters roomSearchParameters)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            string sqlQuery = $@"SELECT r.[Id],[Floor],[Capacity],[ImageUrl],[HasJacuzzi],[HasBreakfast],[RoomTypeId]
                ,r.[Location],r.[Name]
                ,rt.Name as RoomTypeName FROM [SampleHotelDb].[dbo].[Room] r
                inner join RoomType rt on rt.Id = r.RoomTypeId
                where r.Id Not in (select RoomId from  RoomReserve where [From] >= @From and [To] <= @To)
               ";

            using var connection = _context.CreateConnection();

            var roomList = await connection.QueryAsync<RoomDto>(sqlQuery, new
            {
                From = DateTime.Now.Date,
                To = DateTime.Now.Date.AddDays(1)
            });

            foreach (var room in roomList)
            {

                sqlQuery = @"select RoomId, UserId From [dbo].[LikedRoom] where UserId= @userId and RoomId = @roomId";
                var data = await connection.QuerySingleOrDefaultAsync<LikedRoom>(sqlQuery, new { 
                    userId,
                    roomId = room.Id
                });
                if(data != null)
                    room.IsLiked = true;
            }

            var roomVm = new AvailableRoomViewModel
            {
                RoomList = roomList,
                SearchParameters = roomSearchParameters
            };

            return View(roomVm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> FilterAvailableRoom(AvailableRoomViewModel roomSearchParameters)
        {
            List<string> whereConditions = new();

            if (roomSearchParameters.SearchParameters.From.HasValue ||
                roomSearchParameters.SearchParameters.To.HasValue)
            {
                roomSearchParameters.SearchParameters.From ??= DateTime.MinValue;
                roomSearchParameters.SearchParameters.To ??= DateTime.MaxValue;
                whereConditions.Add("r.Id Not in (select RoomId from  RoomReserve where [From] >= @From and [To] <= @To)");
            }

            if (roomSearchParameters.SearchParameters.Capacity > 0)
                whereConditions.Add("Capacity >= 0");

            if (roomSearchParameters.SearchParameters.HasBreakfast.HasValue)
                whereConditions.Add($"HasBreakfast = {(roomSearchParameters.SearchParameters.HasBreakfast.Value)}");

            if (roomSearchParameters.SearchParameters.HasJacuzzi.HasValue)
                whereConditions.Add($"HasJacuzzi = {(roomSearchParameters.SearchParameters.HasJacuzzi.Value)}");

            if (roomSearchParameters.SearchParameters.RoomTypeId > 0)
                whereConditions.Add($"RoomTypeId = {roomSearchParameters.SearchParameters.RoomTypeId}");

            var where = whereConditions.Any() ? "WHERE " + string.Join(" AND ", whereConditions) : string.Empty;

            string sqlQuery = $@"SELECT r.[Id],[Floor],[Capacity],[ImageUrl],[HasJacuzzi],[HasBreakfast],[RoomTypeId]
                ,r.[Location],r.[Name]
                ,rt.Name as RoomTypeName FROM [SampleHotelDb].[dbo].[Room] r
                inner join RoomType rt on rt.Id = r.RoomTypeId
                {where}";

            using var connection = _context.CreateConnection();

            var roomList = await connection.QueryAsync<RoomDto>(sqlQuery, new
            {
                roomSearchParameters.SearchParameters.From,
                roomSearchParameters.SearchParameters.To
            });

            var roomVm = new AvailableRoomViewModel
            {
                RoomList = roomList,
                SearchParameters = roomSearchParameters.SearchParameters
            };

            return View("AvailableRoom", roomVm);
        }

        [HttpGet]
        [Authorize]
        public IActionResult NewRoom()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Room(int id)
        {
            string sqlQuery = @"SELECT [Id],[Name],[Location],[Floor],[Capacity],[ImageUrl]
                  ,[HasJacuzzi],[HasBreakfast],[RoomTypeId]
                   FROM [SampleHotelDb].[dbo].[Room] where Id = @roomId";

            using var connection = _context.CreateConnection();
            var reservedRoom = await connection
                .QuerySingleOrDefaultAsync<RoomViewModel>(sqlQuery, new { roomId = id });

            return View(reservedRoom);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Room(RoomViewModel model)
        {
            if (model.From >= model.To)
            {
                ModelState.AddModelError("1", "invalid date range");
                return View(model);
            }

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            string sqlQuery = @"INSERT INTO [dbo].[RoomReserve]([RoomId],[From],[To],[UserId]) 
                                VALUES(@roomId, @from, @to, @userId)";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sqlQuery, new
            {
                roomId = model.Id,
                from = model.From,
                to = model.To,
                userId
            });

            return RedirectToAction("ReservedRoomList");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ReservedRoomList()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool isAdmin = User.Claims.Any(e => e.Value == "Admin");
            string sqlQuery = $@"SELECT r.[Id],[Name],[Location],[Floor],[Capacity],[ImageUrl]
                  ,[HasJacuzzi],[HasBreakfast],[RoomTypeId], rr.Id AS RoomReserveId, rr.[From], rr.[to]
                   FROM [SampleHotelDb].[dbo].[Room] r
                   inner join RoomReserve rr on rr.RoomId = r.Id {(!isAdmin ? "where rr.UserId = @userId" : "")} ";

            using var connection = _context.CreateConnection();
            var reservedRoom = await connection
                .QueryAsync<ReserveRoomDto>(sqlQuery, new { userId = userId });

            var vm = new ReserveRoomListViewModel
            {
                ReserveRooms = reservedRoom
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CancelReservation(int id)
        {
            string sqlQuery = "Delete FROM [dbo].[RoomReserve] where Id = @id";

            using var connection = _context.CreateConnection();
            var reservedRoom = await connection.ExecuteAsync(sqlQuery, new { id });

            return RedirectToAction("ReservedRoomList");
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ReservedRoom(int id)
        {
            string sqlQuery = @"SELECT r.[Id],[Name],[Location],[Floor],[Capacity],[ImageUrl]
                  ,[HasJacuzzi],[HasBreakfast],[RoomTypeId], rr.Id AS RoomReserveId, rr.[From], rr.[to]
                   FROM [SampleHotelDb].[dbo].[Room] r
                   inner join RoomReserve rr on rr.RoomId = r.Id where rr.Id = @roomId";

            using var connection = _context.CreateConnection();
            var reservedRoom = await connection
                .QuerySingleOrDefaultAsync<ReserveRoomViewModel>(sqlQuery, new { roomId = id });

            return View(reservedRoom);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ReservedRoom(ReserveRoomViewModel model)
        {
            ModelState.Clear();
            if (model.From >= model.To)
            {
                ModelState.AddModelError("1", "Invalid date range");
                return View(model);
            }

            string sqlQuery = @"update RoomReserve set [From] = @From, [To] = @To where Id = @RoomReserveId";

            using var connection = _context.CreateConnection();
            var reservedRoom = await connection
                .ExecuteAsync(sqlQuery, new
                {
                    model.RoomReserveId,
                    model.From,
                    model.To
                });

            return RedirectToAction("ReservedRoomList");
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> NewRoom(CreateRoomViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("NewRoom", model);
            }

            var sql = @"INSERT INTO [dbo].[Room]([Name],[Location],[Floor],[Capacity],[ImageUrl],[HasJacuzzi],[HasBreakfast],[RoomTypeId])
                        VALUES(@Name,@Location,@Floor,@Capacity,@ImageUrl,@HasJacuzzi,@HasBreakfast,@RoomTypeId)";

            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, model);

            var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", model.Image.FileName);
            using FileStream stream = new FileStream(path, FileMode.Create);
            {
                await model.Image.CopyToAsync(stream);
                stream.Close();
            }

            return RedirectToAction("AvailableRoom", "Panel");
        }

        [HttpGet]
        [Authorize]
        public IActionResult NewStaff()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> StaffList()
        {
            string sqlQuery = $@"SELECT e.[Id],[FirstName],[LastName],[PhoneNumber]
                  ,[City],[Street],[EmployeePositionId],[ImageUrl], ep.Name AS EmployeePositionName
              FROM [SampleHotelDb].[dbo].[Employee] e
              inner join EmployeePosition ep on ep.Id = e.EmployeePositionId
               ";

            using var connection = _context.CreateConnection();

            var employeeList = await connection.QueryAsync<EmployeeDto>(sqlQuery);

            var roomVm = new EmployeeListViewModel
            {
                EmployeeList = employeeList,
            };

            return View(roomVm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("NewStaff", model);
            }

            var sql = @"INSERT INTO [dbo].[Employee]([FirstName],[LastName],[PhoneNumber],[City],[Street],[EmployeePositionId],[ImageUrl],DependentFirstName,DependentLastName,DependentRelation)
                       VALUES(@FirstName,@LastName,@PhoneNumber,@City,@Street,@EmployeePositionId,@ImageUrl,
@DependentFirstName, @DependentLastName, @DependentRelation)";

            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, model);

            var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", model.Image.FileName);
            using FileStream stream = new FileStream(path, FileMode.Create);
            {
                await model.Image.CopyToAsync(stream);
                stream.Close();
            }

            return RedirectToAction("StaffList", "Panel");
        }
    }
}
