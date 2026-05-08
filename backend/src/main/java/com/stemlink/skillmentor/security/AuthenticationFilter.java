package com.stemlink.skillmentor.security;

import jakarta.annotation.Nonnull;
import jakarta.servlet.FilterChain;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import lombok.RequiredArgsConstructor;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

@Component
@RequiredArgsConstructor
public class AuthenticationFilter extends OncePerRequestFilter {

    private final TokenValidator tokenValidator;

    @Override
    protected void doFilterInternal(
            @Nonnull HttpServletRequest request,
            @Nonnull HttpServletResponse response,
            @Nonnull FilterChain filterChain
    ) throws ServletException, IOException {

        String token = extractToken(request);

        System.out.println("TOKEN FOUND: " + (token != null));

        if (token == null) {
            filterChain.doFilter(request, response);
            return;
        }

        // validate ONCE
        boolean isValid = tokenValidator.validateToken(token);
        System.out.println("TOKEN VALID: " + isValid);

        if (!isValid) {
            filterChain.doFilter(request, response);
            return;
        }

        String userId = tokenValidator.extractUserId(token);
        String email = tokenValidator.extractEmail(token);
        String firstName = tokenValidator.extractFirstName(token);
        String lastName = tokenValidator.extractLastName(token);

        UserPrincipal userPrincipal =
                new UserPrincipal(userId, email, firstName, lastName);

        List<String> roles = tokenValidator.extractRoles(token);
        if (roles == null) roles = new ArrayList<>();

        List<GrantedAuthority> authorities =
                roles.stream()
                        .map(role -> new SimpleGrantedAuthority("ROLE_" + role))
                        .collect(Collectors.toList());

        UsernamePasswordAuthenticationToken authentication =
                new UsernamePasswordAuthenticationToken(
                        userPrincipal,
                        null,
                        authorities
                );

        SecurityContextHolder.getContext().setAuthentication(authentication);

        System.out.println("AUTH SUCCESS FOR USER: " + userId);

        filterChain.doFilter(request, response);
    }

    private String extractToken(HttpServletRequest request) {
        String bearerToken = request.getHeader("Authorization");
        if (bearerToken != null && bearerToken.startsWith("Bearer ")) {
            return bearerToken.substring(7);
        }
        return null;
    }
}