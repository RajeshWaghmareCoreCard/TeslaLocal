namespace CoreCard.Tesla.Falcon.DbRepository
{
    public class FalconSqlCommands
    {
        public static string GetCardByToken = @"
                SELECT 
                    card_id, account_id, customer_id, card_token_id, card_expiration_date, bin, card_last4, product_id, institution_id, program_manager_id, activated_date, activated, credit_limit, current_balance, daily_limit_amount, daily_limit_date, daily_limit_count, current_daily_limit_amount, current_daily_limit_count, cash_balance, cash_limit, daily_cash_limit_amount, daily_cash_limit_date, daily_cash_limit_count, current_daily_cash_limit_amount, current_daily_cash_limit_count, card_status, created_at, created_by, updated_by, updated_at, card_type
                FROM
                    cards nolock
                WHERE 
                    card_token_id=@card_token_id";
        public static string GetCustomerById = @"
                SELECT 
                    customer_id, institution_id, customer_name, customer_address, ssn, active, created_at, created_by, updated_by, updated_at
                FROM
                    customers
                WHERE 
                    customer_id=@customer_id
                For Update";
        public static string GetAccountById = @"
                SELECT 
                    account_id, account_number, current_balance, credit_limit, product_id, customer_id, account_status, created_at, created_by, updated_by, updated_at, daily_limit_amount, daily_limit_date, daily_limit_count, current_daily_limit_amount, current_daily_limit_count, cash_balance, cash_credit_limit
                FROM
                    accounts
                WHERE 
                    account_id= @account_id
                For Update";
        public static string GetAllProductAdcs = @"
                SELECT 
                    product_adc_id, product_id, adc_id, internal_response_code, active
                FROM
                    product_adcs
                WHERE
                    active= true";
        public static string GetAllCardAdcsByCardId = @"
                SELECT 
                    card_adc_id, card_id, adc_id, response_code, internal_response_code, continue_on_timeout, active
                FROM
                    card_adcs
                WHERE
                    card_id=@card_id and 
                    active= true
                FOR Update";
        public static string GetAllAccountAdcsByAccountId = @"
                SELECT 
                    account_adc_id, account_id, adc_id, response_code, internal_response_code, continue_on_timeout, active
                FROM
                    account_adcs
                WHERE
                    account_id=@account_id and 
                    active= true
                For Update";

    }
}