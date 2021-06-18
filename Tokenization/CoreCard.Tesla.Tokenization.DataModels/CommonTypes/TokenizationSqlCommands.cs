namespace CoreCard.Tesla.Tokenization.DataModels.CommonTypes
{
    public abstract class TokenizationSqlCommands
    {
        public const string GetModuleSessionCommand = @"
                                                        Select 
                                                            module_id,session_id,key_details,session_expiry_date 
                                                        FROM 
                                                            module_sessions nolock 
                                                        WHERE 
                                                            session_id =@session_id";


        public const string InsertModuleSessionCommand = @"
                                                        INSERT INTO module_sessions
                                                            (session_id, 
                                                            key_details, 
                                                            module_id, 
                                                            created_at,
                                                            created_by,
                                                            updated_date,
                                                            updated_by,
                                                            session_expiry_date) 
                                                        VALUES
                                                            (@session_id,
                                                            @key_details,
                                                            @module_id,
                                                            @created_at,
                                                            @Created_by,
                                                            @updated_date,
                                                            @updated_by,
                                                            @session_expiry_date)";

        public const string GetModuleKeyCommand = @"
                                                    SELECT 
                                                        module_key_id, public_key, active, module_id 
                                                    FROM 
                                                        module_keys nolock 
                                                    where 
                                                        module_key_id = @module_key_id";

        public const string GetCardTokenByCardHashCommand = @"
                                                    SELECT 
                                                        card_token_id, institution_id, network_name,card_bin,card_last4,card_expiration,active
                                                    FROM 
                                                        card_tokens nolock 
                                                    WHERE 
                                                        card_hash = @card_hash";

        public const string GetCardTokenCommand = @"
                                                    SELECT 
                                                        card_token_id, institution_id, network_name,card_bin,card_last4,card_expiration,active, encrypted_card 
                                                    FROM 
                                                        card_tokens nolock 
                                                    WHERE 
                                                        card_token_id = @card_token_id";
        public const string InsertCardTokenCommand = @"
                                                    INSERT INTO card_tokens
                                                        (card_token_id,
                                                         institution_id,
                                                         network_name,
                                                         card_bin,
                                                         card_last4,
                                                         card_expiration,
                                                         active,
                                                         card_hash,
                                                         encrypted_card) 
                                                    VALUES
                                                        (@card_token_id,
                                                         @institution_id,
                                                         @network_name,
                                                         @card_bin,
                                                         @card_last4,
                                                         @card_expiration,
                                                         @active,
                                                         @card_hash,
                                                         @encrypted_card)";
        public const string InsertOtherTokenCommand = @"
                                                    INSERT INTO other_tokens
                                                        (other_token_id,
                                                         institution_id,
                                                         token_hash,
                                                         encrypted_data,
                                                         token_family_id,
                                                         active,
                                                         hsm_active_key_id
                                                         ) 
                                                    VALUES
                                                        (@other_token_id,
                                                         @institution_id,
                                                         @token_hash,
                                                         @encrypted_data,
                                                         @token_family_id,
                                                         @active,
                                                         @hsm_active_key_id)";
        public const string GetOtherTokenCommand = @"
                                                    SELECT 
                                                        other_token_id, institution_id, token_hash, encrypted_data, active, token_family_id
                                                    FROM 
                                                        other_tokens nolock 
                                                    WHERE 
                                                        other_token_id = @other_token_id";
        public const string GetOtherTokenByHashCommand = @"
                                                    SELECT 
                                                        other_token_id, institution_id, token_hash, encrypted_data, active, token_family_id
                                                    FROM 
                                                        other_tokens nolock 
                                                    WHERE 
                                                        token_hash = @token_hash";
        public const string GetModulePermissionCommand = @"
                                                    SELECT 
                                                        module_permission_id, module_id, token_family_id, tokenization_allowed, tokenization_notify, detokenization_notify, detokenization_allowed
                                                    FROM 
                                                        module_permissions nolock 
                                                    WHERE 
                                                        module_id = @module_id
                                                        and
                                                        token_family_id =@token_family_id";
    }
}