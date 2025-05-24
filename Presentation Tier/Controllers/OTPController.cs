using Interface_Tier.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Presentation_Tier.RequestDTOs;

namespace Presentation_Tier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController(IOTPService service) : ControllerBase
    {
        [HttpPost("AddOTP")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> AddOTP(AddOTPRequestDTO dTO)
        {
            if (string.IsNullOrEmpty(dTO.Email))
            {
                return BadRequest("Email is requierd.");
            }
            try
            {
                bool Result = await service.AddOTP(dTO.Email); ;

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred in add otp");
                }


                return NoContent();
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpPost("VerifyOTP")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> VerifyOTP(VerifyOTPRequestDTO dTO)
        {
            if (string.IsNullOrEmpty(dTO.Email) || string.IsNullOrEmpty(dTO.otp))
            {
                return BadRequest("Email and otp are requierd.");
            }
            try
            {
                bool Result = await service.VerifyOTP(dTO.Email, dTO.otp); ;

                if (!Result)
                {
                    return NotFound("OTP is not correct");
                }



                return NoContent();
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
