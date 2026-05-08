package com.stemlink.skillmentor.security;

import com.auth0.jwk.Jwk;
import com.auth0.jwk.JwkProvider;
import com.auth0.jwk.UrlJwkProvider;
import com.auth0.jwt.JWT;
import com.auth0.jwt.algorithms.Algorithm;
import com.auth0.jwt.interfaces.DecodedJWT;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Primary;
import org.springframework.stereotype.Component;

import java.net.URL;
import java.security.PublicKey;
import java.util.List;

@Slf4j
@Component
@Primary
public class ClerkValidator implements TokenValidator {

    private final JwkProvider jwkProvider;

    public ClerkValidator(@Value("${clerk.jwks.url}") String clerkJwksUrl) {
        try {
            this.jwkProvider = new UrlJwkProvider(new URL(clerkJwksUrl));
        } catch (Exception e) {
            log.error("Failed to init JWK provider", e);
            throw new RuntimeException(e);
        }
    }

    @Override
    public boolean validateToken(String token) {
        try {
            DecodedJWT jwt = JWT.decode(token);

            String kid = jwt.getKeyId();
            if (kid == null) return false;

            Jwk jwk = jwkProvider.get(kid);
            PublicKey publicKey = jwk.getPublicKey();

            Algorithm algorithm =
                    Algorithm.RSA256((java.security.interfaces.RSAPublicKey) publicKey, null);

            JWT.require(algorithm).build().verify(token);

            return true;

        } catch (Exception e) {
            log.error("Token validation failed: {}", e.getMessage());
            return false;
        }
    }

    @Override
    public String extractUserId(String token) {
        return JWT.decode(token).getSubject();
    }

    @Override
    public List<String> extractRoles(String token) {
        try {
            List<String> roles = JWT.decode(token)
                    .getClaim("roles")
                    .asList(String.class);

            return roles != null ? roles : List.of();

        } catch (Exception e) {
            return List.of();
        }
    }

    @Override
    public String extractFirstName(String token) {
        return JWT.decode(token).getClaim("firstName").asString();
    }

    @Override
    public String extractLastName(String token) {
        return JWT.decode(token).getClaim("lastName").asString();
    }

    @Override
    public String extractEmail(String token) {
        return JWT.decode(token).getClaim("email").asString();
    }
}